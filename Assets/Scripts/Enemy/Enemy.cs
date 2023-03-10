using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private ParticleSystem _particalSystem;

    // link to the higher mind
    public EnemyManager EnemyManager;

    // link another scripts for get information
    [SerializeField] private EnemyDefense _enemyDefence;

    // field:
    // about health
    public float MaxHealth;
    public float CurrentHealth;
    private bool _isAlive = true;
    [SerializeField] private bool _canHealHimself;
    [SerializeField] private bool _canHealAnotherEnemy;
    [SerializeField] private bool _canTransferDamage;

    // position
    [SerializeField] private bool _canChangePosition;

    [SerializeField] private float _abilityTimer;

    public void Initialize(EnemyManager EnemyManager, float maxHealth, bool canHealHimself, bool canHealAnotherEnemy, bool canTransferDamage, bool canChangePosition)
    {
        this.EnemyManager = EnemyManager;

        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;

        _canHealHimself = canHealHimself;
        _canHealAnotherEnemy = canHealAnotherEnemy;
        _canTransferDamage = canTransferDamage;
        _canChangePosition = canChangePosition;

    }

    private void Awake()
    {
        _particalSystem.Stop();

        if (EnemyManager == null) 
        {
            EnemyManager = GetComponentInParent<EnemyManager>();
        }
    }

    private void Update()
    {
        if (_abilityTimer > EnemyManager.AbilityPeriod)
        {
            if (CanUseAbility())
            {
                if (CurrentHealth != MaxHealth)
                {
                    Heal(this);
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
        }
    }

    public void TakeDamage(float damageValue, AttackType AttackType = AttackType.Default)
    {
        if (AttackType == AttackType.Special || !_enemyDefence.GetDefenceState())
        {
            CurrentHealth -= damageValue;

            if (_isAlive = CheckDeath())
            {
                Death();
            }
        }
    }

    private bool CheckDeath()
    {
        return CurrentHealth <= 0 ? true : false;
    }

    private void Death()
    {
        GameplayEventManager.SendEnemyKilled();
        EnemyManager.EnemyCount--;
        EnemyManager.DeleteEnemyFromList(this);
        Destroy(gameObject);
        //Debug.Log($"{gameObject.name} dead");
    }

    public void Heal(Enemy target)
    {
        if (target.CurrentHealth != target.MaxHealth)
        {
            target.CurrentHealth = target.MaxHealth;
            _abilityTimer = 0f;
            target._particalSystem.Play();
        }
    }

    public void TransferDamage(Enemy target, float damageValue)
    {
        target.TakeDamage(damageValue);
        _abilityTimer = 0f;
    }

    private bool CanUseAbility()
    {
        return Random.Range(0, 100) <= 60 ? true : false;
    }
}
