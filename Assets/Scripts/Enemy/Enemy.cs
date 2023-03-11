using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private GameObject _particalSystemHealHimself;
    [SerializeField] private GameObject _particalSystemHealAnother;

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
        if (EnemyManager == null)
        {
            EnemyManager = GetComponentInParent<EnemyManager>();
        }
    }

    private void Update()
    {
        if (_abilityTimer > EnemyManager.AbilityPeriod)
        {
            if (_canHealHimself)
            {
                Heal();
            }
            else if (_canHealAnotherEnemy)
            {
                HealAnother();
            }
            _abilityTimer = 0;
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
            TakeDamage(ball);
        }
    }

    public void TakeDamage(Ball ball)
    {
        if (ball.AttackType == AttackType.Special || !_enemyDefence.GetDefenceState())
        {
            if (_abilityTimer > EnemyManager.AbilityPeriod && _canTransferDamage)
            {
                EnemyManager.TransferDamage(this, ball);
            }
            else
            {
                CurrentHealth -= ball.DamageValue;
            }

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
    }

    public void Heal()
    {
        StartCoroutine(PlayParticle(_particalSystemHealHimself));
        CurrentHealth++;
    }

    public void HealAnother()
    {
        bool doHeal = EnemyManager.HealEnemy(this);
        if (doHeal) StartCoroutine(PlayParticle(_particalSystemHealAnother));
    }

    private IEnumerator PlayParticle(GameObject particle)
    {
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2);
        particle.GetComponent<ParticleSystem>().Stop();
        particle.SetActive(false);
    }

    public IEnumerator UseAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(EnemyManager.AbilityPeriod);

        while (true)
        {
            yield return wait;
            if (_canHealHimself && CurrentHealth < MaxHealth)
            {
                Heal();
            }
            else if (_canHealAnotherEnemy)
            {
                HealAnother();
            }
        }
    }
}
