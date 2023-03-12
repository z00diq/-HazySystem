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
    [SerializeField] private GameObject _particalSystemTransferIn;
    [SerializeField] private GameObject _particalSystemTransferOut;

    [SerializeField] private List<GameObject> HP;
    [SerializeField] private GameObject HealVisualOnObject;

    // link to the higher mind
    public EnemyManager EnemyManager;

    // link another scripts for get information
    [SerializeField] private EnemyDefense _enemyDefence;

    // field:
    // about health
    public float MaxHealth;
    public float CurrentHealth;

    [SerializeField] private bool _canHealHimself;
    [SerializeField] private bool _canHealAnotherEnemy;
    [SerializeField] private bool _canTransferDamage;

    [SerializeField] private float _abilityTimer;

    public void Initialize(EnemyManager EnemyManager, float maxHealth, bool canHealHimself, bool canHealAnotherEnemy, bool canTransferDamage)
    {
        this.EnemyManager = EnemyManager;

        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;

        _canHealHimself = canHealHimself;
        _canHealAnotherEnemy = canHealAnotherEnemy;
        _canTransferDamage = canTransferDamage;

        ShowHP();
        ShowCanHeal();
    }

    private void Awake()
    {
        if (EnemyManager == null)
        {
            EnemyManager = FindObjectOfType<EnemyManager>();
        }

        ShowHP();
        ShowCanHeal();
    }

    private void Update()
    {
        if (_abilityTimer > EnemyManager.AbilityPeriod && _abilityTimer < EnemyManager.AbilityPeriod * 2)
        {
            if (_canHealHimself)
            {
                Heal();
            }
            else if (_canHealAnotherEnemy)
            {
                HealAnother();
            }
            _abilityTimer += Time.deltaTime;
        }
        else if(_abilityTimer > EnemyManager.AbilityPeriod * 2)
        {
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
                if (!EnemyManager.TransferDamage(this, ball))
                {
                    CurrentHealth -= ball.DamageValue;
                    ShowHP();
                }
            }
            else
            {
                CurrentHealth -= ball.DamageValue;
                ShowHP();
            }

            if (CheckDeath())
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
        if (CurrentHealth < MaxHealth)
        {
            StartCoroutine(PlayParticle(_particalSystemHealHimself));
            CurrentHealth++;
            _abilityTimer = 0;
            ShowHP();
        }
    }

    public void HealAnother()
    {
        bool doHeal = EnemyManager.HealEnemy(this);
        if (doHeal) StartCoroutine(PlayParticle(_particalSystemHealAnother));
        _abilityTimer = 0;
    }

    public void TransferIN()
    {
        StartCoroutine(PlayParticle(_particalSystemTransferIn));
    }

    public void TransferOUT()
    {
        StartCoroutine(PlayParticle(_particalSystemTransferOut));
    }

    private IEnumerator PlayParticle(GameObject particle)
    {
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(4);
        particle.GetComponent<ParticleSystem>().Stop();
        particle.SetActive(false);
    }

    private void ShowHP()
    {
        for (int i = 0; i < (int)MaxHealth; i++)
        {
            if (i < (int)CurrentHealth)
            {
                HP[i].SetActive(true);
            }
            else
            {
                HP[i].SetActive(false);
            }
        }
    }

    private void ShowCanHeal()
    {
        if (_canHealHimself || _canHealAnotherEnemy)
        {
            HealVisualOnObject.SetActive(true);
        }
    }
}
