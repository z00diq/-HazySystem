using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    // link to the higher mind
    private EnemyManager _enemyManager;

    // field:
    // about health
    [SerializeField] private float _maxHealth;
    public float CurrentHealth;
    private bool _isAlive = true;
    [SerializeField] private bool _canDefence;
    [SerializeField] private bool _haveDefence;
    [SerializeField] private bool _canHealHimself;
    [SerializeField] private bool _canHealAnotherEnemy;
    [SerializeField] private bool _canTransferDamage;

    // influenct on player
    [SerializeField] private bool _canSlowBall;

    // position
    [SerializeField] private bool _canChangePosition;

    [SerializeField] private float _abilityTimer;

    // about reproduction
    [SerializeField] private bool _canReproduse;
    [SerializeField] private bool _haveFastReproduction;
    [SerializeField] private float _reproductionPeriod;
    private float _reproductionTimer;
    [SerializeField] private Color _reproductionColor;


    [SerializeField] private bool _canShowRays;

    [SerializeField] private static float SpawnDistance = 0.2f;

    [SerializeField] private List<Vector3> _rayDirections = new List<Vector3>();
    private List<Ray> _rays = new List<Ray>();

    public void Initialize(EnemyManager EnemyManager, bool fastReproduction, float maxHealth, bool haveInvul, bool canHealHimself, bool canHealAnotherEnemy, bool canTransferDamage, bool canChangePosition, bool canSlowBall)
    {
        _enemyManager = EnemyManager;

        _haveFastReproduction = fastReproduction;

        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;

        _canDefence = haveInvul;

        _canHealHimself = canHealHimself;
        _canHealAnotherEnemy = canHealAnotherEnemy;
        _canTransferDamage = canTransferDamage;
        _canChangePosition = canChangePosition;

        _canSlowBall = canSlowBall;

        SetupReproductionPeriod(fastReproduction);
    }

    private void Awake()
    {
        OnBorn?.Invoke(_hazzard);
        // add randomness for reproduction period for greater unevenness of the appearance new cells
        _reproductionPeriod += Random.Range(-0.5f, 1f);
        // defence of mistake: negative value in reproductionPeriod;
        _reproductionPeriod = Mathf.Clamp(_reproductionPeriod, 1, 100);

        CreateRays();

        //Debug.Log("� �������");
    }

    private void CreateRays()
    {
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            _rays.Add(new Ray(transform.position, _rayDirections[i] * SpawnDistance));
        }
    }

    private void ShowRays()
    {
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            Debug.DrawRay(transform.position, _rayDirections[i] * SpawnDistance);
        }
    }

    private void Update()
    {
        if (_canReproduse)
        {
            _reproductionTimer += Time.deltaTime;
            if (_reproductionTimer >= _reproductionPeriod)
            {
                Vector3 newCellPosition = GetPositionForNewCells();
                if (newCellPosition != Vector3.zero)
                {
                    _enemyManager.Reproduce(newCellPosition);
                    //Debug.Log($"{gameObject.name} reproduce");
                }
                _reproductionTimer = 0;
            }
        }

        if (_abilityTimer > _enemyManager.AbilityPeriod)
        {
            if (CanUseAbility())
            {
                /*
                if (CurrentHealth != _maxHealth)
                {
                    Heal(this);
                }
                */
                if (_canDefence && !_haveDefence)
                {
                    StartCoroutine(StayInvul());
                }
            }
        }
        else
        {
            _abilityTimer += Time.deltaTime;
        }

        if (_canShowRays)
        {
            ShowRays();
        }
    }

    private IEnumerator ReproduceCoroutine(bool canReproduce)
    {
        WaitForSeconds wait = new WaitForSeconds(_reproductionPeriod);

        while (canReproduce)
        {
            Vector3 newCellPosition = GetPositionForNewCells();
            if (newCellPosition == Vector3.zero)
            {
                _enemyManager.Reproduce(newCellPosition);
            }

            yield return wait;
        }
    }


    private Vector3 GetPositionForNewCells()
    {
        Vector3 vectorToNewCells = Vector3.zero;
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            Ray ray = new Ray(transform.position, transform.TransformDirection(_rayDirections[i])); 

            if (Physics.Raycast(ray, out RaycastHit hit, SpawnDistance*Mathf.Sqrt(2)))
            {
                if (hit.collider)
                {
                    //Debug.Log($"{gameObject.name} hit {hit.transform.gameObject.name} in {_rayDirections[i]} direction");
                    continue;
                }
            }
            else
            {
                //Debug.Log($"Direction: {_rayDirections[i] * RayRange} empty! We can reproduce on this position!");
                vectorToNewCells = _rayDirections[i] * SpawnDistance;
                return transform.position + vectorToNewCells;
            }
        }

        return Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Ball>() is Ball ball)
        {
            TakeDamage(ball.DamageValue, ball.AttackType);

            if (_canSlowBall)
            {
                //StartCoroutine(ball.SlowBallForTime(2, 5));
            }
        }
    }

    public void TakeDamage(float damageValue, AttackType AttackType = AttackType.Default)
    {
        if (AttackType == AttackType.Special || !_haveDefence)
        {
            CurrentHealth -= damageValue;

            if (_isAlive = CheckDeath())
            {
                Destroy(gameObject);
                //Debug.Log($"{gameObject.name} dead");
            }
        }
    }

    private bool CheckDeath()
    {
        return CurrentHealth <= 0 ? true : false;
    }

    public void SetupReproductionPeriod(bool isActive)
    {
        float offset = Random.Range(-0.5f, 1f);

        _reproductionPeriod = isActive ? _enemyManager.ReproductionPeriod / 2 + offset: _enemyManager.ReproductionPeriod + offset;
        _reproductionPeriod = Mathf.Clamp(_reproductionPeriod, 1, 10);
    }

    public void Heal(Enemy target)
    {
        if (target.CurrentHealth != target._maxHealth)
        {
            target.CurrentHealth = target._maxHealth;
            _abilityTimer = 0f;
        }
    }

    public void TransferDamage(Enemy target, float damageValue)
    {
        target.TakeDamage(damageValue);
        _abilityTimer = 0f;
    }

    private bool CanUseAbility()
    {
        return Random.Range(0, 100) > 60 ? true : false;
    }

    private IEnumerator StayInvul()
    {
        _haveDefence = true;
        _renderer.material.color = Color.white;

        _abilityTimer = 0f;

        yield return new WaitForSeconds(5);

        _haveDefence = false;
        _renderer.material.color = Color.red;
    }
}
