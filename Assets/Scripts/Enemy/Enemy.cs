using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    // link to the higher mind
    public EnemyManager EnemyManager;

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

    public void Initialize(EnemyManager EnemyManager, float maxHealth, bool haveInvul, bool canHealHimself, bool canHealAnotherEnemy, bool canTransferDamage, bool canChangePosition, bool canSlowBall)
    {
        this.EnemyManager = EnemyManager;

        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;

        _canDefence = haveInvul;

        _canHealHimself = canHealHimself;
        _canHealAnotherEnemy = canHealAnotherEnemy;
        _canTransferDamage = canTransferDamage;
        _canChangePosition = canChangePosition;

        _canSlowBall = canSlowBall;

    }

    private void Update()
    {
        if (_abilityTimer > EnemyManager.AbilityPeriod)
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
                GameplayEventManager.SendEnemyKilled();
                Destroy(gameObject);
                //Debug.Log($"{gameObject.name} dead");
            }
        }
    }

    private bool CheckDeath()
    {
        return CurrentHealth <= 0 ? true : false;
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
