using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // link to the higher mind
    private EnemyManager _enemyManager;

    // field:
    // about health
    [SerializeField] private float _maxHealth;
    public float CurrentHealth;
    private bool _isAlive = true;
    private bool _haveInvul;
    private bool _canHealHimself;
    private bool _canHealAnotherEnemy;
    private bool _canTransferDamage;

    // position
    private bool _canChangePosition;

    // about reproduction
    [SerializeField] public bool _canReproduse;
    [SerializeField] private float _reproductionPeriodBase;
    private float _reproductionPeriodCurrent;
    private float _reproductionTimer;
    [SerializeField] private Color _reproductionColor;

    [SerializeField] private float _abilityTimer;

    [SerializeField] private bool _canShowRays;

    [SerializeField] private static float SpawnDistance = 0.2f;

    [SerializeField] private List<Vector3> _rayDirections = new List<Vector3>();
    private List<Ray> _rays = new List<Ray>();

    public void Initialize(EnemyManager EnemyManager, bool fastReproduction, float maxHealth, bool invul, bool canHealHimself, bool canHealAnotherEnemy, bool canTransferDamage, bool canChangePosition)
    {
        _enemyManager = EnemyManager;

        CheckReproductionPeriod(fastReproduction);

        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;

        _haveInvul = invul;

        _canHealHimself = canHealHimself;
        _canHealAnotherEnemy = canHealAnotherEnemy;
        _canTransferDamage = canTransferDamage;
        _canChangePosition = canChangePosition;
    }

    private void Awake()
    {
        CreateRays();

        //Debug.Log("Я родился");
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
            if (_reproductionTimer >= _reproductionPeriodCurrent)
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

        if (_canShowRays)
        {
            ShowRays();
        }
    }

    private IEnumerator ReproduceCoroutine(bool canReproduce)
    {
        WaitForSeconds wait = new WaitForSeconds(_reproductionPeriodCurrent);

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
            TakeDamage(ball.DamageValue);
        }
    }

    public void TakeDamage(float damageValue)
    {
        CurrentHealth -= damageValue;

        if (CheckDeath())
        {
            _isAlive = false;
            Destroy(gameObject);
            //Debug.Log($"{gameObject.name} dead");
        }
    }

    private bool CheckDeath()
    {
        return CurrentHealth <= 0 ? true : false;
    }

    public void CheckReproductionPeriod(bool isActive)
    {
        // add randomness for reproduction period for greater unevenness of the appearance new cells
        _reproductionPeriodBase += _enemyManager.ReproductionPeriodBase + Random.Range(-0.5f, 1f);
        // defence of mistake: negative value in reproductionPeriod;
        _reproductionPeriodBase = Mathf.Clamp(_reproductionPeriodBase, 1, 100);

        _reproductionPeriodCurrent = isActive ? _reproductionPeriodBase / 2 : _reproductionPeriodBase;
    }


}
