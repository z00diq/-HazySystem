using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemiesLevelRules _enemiesRules;

    [SerializeField] private GameObject _enemyPrefab;

    public float SpawnDistance;

    public int EnemyCount;
    public List<Enemy> EnemyList = new List<Enemy>();

    public float ReproductionPeriod;
    public float AbilityPeriod;

    public UnityEvent<bool> ReproduceRuleChanged;
    public UnityEvent<bool> MovingRuleChanged;

    [SerializeField] private bool _canReproduce = true;
    public bool CanReproduce
    {
        get { return _canReproduce; }
        set
        {
            if (_canReproduce != value)
            {
                ReproduceRuleChanged?.Invoke(value);
            }
            _canReproduce = value;
        }
    }

    [SerializeField] private bool _canMoving = true;
    public bool CanMoving
    {
        get { return _canMoving; }
        set
        {
            if (_canMoving != value)
            {
                MovingRuleChanged?.Invoke(value);
            }
            _canMoving = value;
        }
    }

    private void Awake()
    {
        SpawnDistance = _enemiesRules.SpawnDistance;

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            EnemyList.Add(enemy);
        }

        EnemyCount += FindObjectsOfType<Enemy>().Length;

        StartCoroutine(ReproduceStartDelay());
        ReproductionPeriod = _enemiesRules.ReproductionPeriodBase;
        AbilityPeriod = _enemiesRules.AbilityPeriod;

        Reproduce(transform.position);
    }

    private IEnumerator ReproduceStartDelay()
    {
        WaitForSeconds wait = new WaitForSeconds(_enemiesRules.ReproductionDelay);

        CanReproduce = false;
        yield return wait;
        CanReproduce = true;
    }

    public void Reproduce(Vector3 position)
    {
        if (CheckEmptyPlace(position))
        {
            GameObject newCells = Instantiate(_enemyPrefab, position, Quaternion.identity);
            Enemy newCellsEnemy = newCells.GetComponent<Enemy>();
            EnemyReproduce newCellsEnemyReproduce = newCells.GetComponent<EnemyReproduce>();
            EnemyDefense newCellsDefense = newCells.GetComponent<EnemyDefense>();
            EnemySlowBall newCellSlowBall = newCells.GetComponent<EnemySlowBall>();
            EnemyMoving newCellsMoving = newCells.GetComponent<EnemyMoving>();

            bool thisCanFastReproduction = false;
            bool thisCanHaveBiggerHealth = false;
            bool thisCanDefence = false;
            bool thisCanSlowBall = false;
            bool thisCanHealHimself = false;
            bool thisCanHealAnother = false;
            bool thisCanTransferDamage = false;
            bool thisCanChangePosition = false;

            void GetAbilities()
            {
                if (CanHaveAbilities())
                {
                    int AbilityNumber = (int)Random.Range(0, 8);
                    switch (AbilityNumber)
                    {
                        case 0:
                            if (thisCanFastReproduction || !_enemiesRules.CanHaveFastReproduction) GetAbilities();
                            else
                            {
                                thisCanFastReproduction = true;
                            }
                            break;
                        case 1:
                            if (thisCanHaveBiggerHealth || !_enemiesRules.CanHaveBiggerHealth) GetAbilities();
                            else
                            {
                                thisCanHaveBiggerHealth = true;
                            }
                            break;
                        case 2:
                            if (thisCanDefence || !_enemiesRules.CanDefence) GetAbilities();
                            else
                            {
                                thisCanDefence = true;
                            }
                            break;
                        case 3:
                            if (thisCanSlowBall || !_enemiesRules.CanSlowBall) GetAbilities();
                            else
                            {
                                thisCanSlowBall = true;
                            }
                            break;
                        case 4:
                            if (thisCanHealHimself || !_enemiesRules.CanHealHimself) GetAbilities();
                            else
                            {
                                thisCanHealHimself = true;
                            }
                            break;
                        case 5:
                            if (thisCanHealAnother || !_enemiesRules.CanHealAnother) GetAbilities();
                            else
                            {
                                thisCanHealAnother = true;
                            }
                            break;
                        case 6:
                            if (thisCanTransferDamage || !_enemiesRules.CanTransferDamage) GetAbilities();
                            else
                            {
                                thisCanTransferDamage = true;
                            }
                            break;
                        case 7:
                            if (thisCanChangePosition || !_enemiesRules.CanChangePosition) GetAbilities();
                            else
                            {
                                thisCanChangePosition = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            for (int i = 0; i < _enemiesRules.EnemyMaximumAbilities; i++)
            {
                GetAbilities();
            }

            newCellsEnemy.Initialize(this,
                SetupMaxHealth(thisCanHaveBiggerHealth),    // max health
                thisCanHealHimself,                         // can heal himself
                thisCanHealAnother,                         // can heal another
                thisCanTransferDamage                       // can transfer damage
                );

            newCellsEnemyReproduce.Initialize(
                this,
                thisCanFastReproduction                     // fast reproduction
                );

            if (thisCanDefence)
            {
                newCellsDefense.Initialize(true, AbilityPeriod);
            }
            else
            {
                newCellsDefense.enabled = false;
            }

            if (thisCanSlowBall)
            {
                newCellSlowBall.Initialize(true, AbilityPeriod);
            }
            else
            {
                newCellSlowBall.enabled = false;
            }

            if (thisCanChangePosition)
            {
                newCellsMoving.Initialize(true, AbilityPeriod);
            }


            newCells.transform.parent = transform;
            newCells.gameObject.name = "Enemy" + EnemyCount;
            EnemyCount++;
            EnemyList.Add(newCellsEnemy);
        }
    }

    private bool CheckEmptyPlace(Vector3 position)
    {
        if (Physics.CheckBox(position, Vector3.one * (_enemyPrefab.transform.localScale.x*1.1f), Quaternion.identity))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CanHaveAbilities()
    {
        return (int)Random.Range(0, 100) <= _enemiesRules.ChanceGetAbility ? true : false;
    }

    private float SetupMaxHealth(bool value)
    {
        return value ? _enemiesRules.MaximumHealthForCells : _enemiesRules.MinimumHealthForCells;
    }

    public IEnumerator StopReproduce(float time)
    {
        CanReproduce = false;
        yield return new WaitForSeconds(time);
        CanReproduce = true;
    }

    public IEnumerator StopEnemyMoving(float time)
    {
        CanMoving = false;
        yield return new WaitForSeconds(time);
        CanMoving = true;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) StartCoroutine(StopReproduce(1000));
    }

    public bool HealEnemy(Enemy healer)
    {
        if (EnemyList.Count > 2)
        {
            Enemy target;

            int tryToHeal = 3;

            for (var i = 0; i < tryToHeal; i++)
            {
                target = EnemyList[(int)Random.Range(0, EnemyList.Count - 1)];
                if (target != healer && target.CurrentHealth < target.MaxHealth)
                {                    
                    target.Heal();
                    return true;
                }
            }
        }

        return false;
    }

    public bool TransferDamage(Enemy hittenEnemy, Ball ball)
    {
        if (EnemyList.Count > 2)
        {
            Enemy target;
            int tryToTransfer = 3;

            for (var i = 0; i < tryToTransfer; i++)
            {
                target = EnemyList[(int)Random.Range(0, EnemyList.Count - 1)];
                Debug.Log(target.name);
                if (target != hittenEnemy && target.CurrentHealth == target.MaxHealth)
                {
                    target.TakeDamage(ball);
                    target.TransferOUT();
                    hittenEnemy.TransferIN();

                    return true;
                }
            }
        }

        return false;
    }

    public void DeleteEnemyFromList(Enemy deadEnemy)
    {
        EnemyList.Remove(deadEnemy);
    }
}
