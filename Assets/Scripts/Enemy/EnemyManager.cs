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

    [SerializeField] private bool _canReproduce;
    public bool CanReproduce
    {
        get => _canReproduce;
        set
        {
            if (_canReproduce != value)
            {
                ReproduceRuleChanged?.Invoke(value);
            }
            _canReproduce = value;
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

        CanReproduce = true;
        ReproductionPeriod = _enemiesRules.ReproductionPeriodBase;
        AbilityPeriod = _enemiesRules.AbilityPeriod;

        Reproduce(transform.position);
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

            newCellsEnemy.Initialize(this,
                SetupMaxHealth(),                       // max health
                CanHaveAbilities(),                     // can heal himself
                CanHaveAbilities(),                     // can heal another
                CanHaveAbilities(),                     // can transfer damage
                CanHaveAbilities()                      // can change position
                );

            newCellsEnemyReproduce.Initialize(
                this,
                CanHaveAbilities()                      // fast reproduction
                );

            if (CanHaveAbilities())
            {
                newCellsDefense.Initialize(true, AbilityPeriod);
            }
            else
            {
                newCellsDefense.enabled = false;
            }

            if (CanHaveAbilities())
            {
                newCellSlowBall.Initialize(true, AbilityPeriod);
            }
            else
            {
                newCellSlowBall.enabled = false;
            }


            newCells.transform.parent = transform;
            newCells.gameObject.name = "Enemy" + EnemyCount;
            EnemyCount++;
            EnemyList.Add(newCellsEnemy);
        }
    }

    private bool CheckEmptyPlace(Vector3 position)
    {
        if (Physics.CheckBox(position, new Vector3(0.06f, 0.06f, 0.05f), Quaternion.identity))
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

    private float SetupMaxHealth()
    {
        return (int)Random.Range(0, 100) <= _enemiesRules.ChanceGetAbility ? _enemiesRules.MaximumHealthForCells : _enemiesRules.MinimumHealthForCells;
    }

    public IEnumerator StopReproduce(float time)
    {
        CanReproduce = false;

        WaitForSeconds wait = new WaitForSeconds(time);
        yield return wait;

        CanReproduce = true;
    }

    public IEnumerator StopEnemyMoving(float time)
    {


        yield return null;
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
                    healer.HealAnother();
                    target.Heal();

                    return true;
                }
            }
        }

        return false;
    }

    public void TransferDamage(Enemy hittenEnemy, Ball ball)
    {
        if (EnemyList.Count > 2)
        {
            Enemy target;
            int tryToTransfer = 3;

            for (var i = 0; i < tryToTransfer; i++)
            {
                target = EnemyList[(int)Random.Range(0, EnemyList.Count - 1)];
                if (target != hittenEnemy && target.CurrentHealth < target.MaxHealth)
                {
                    target.TakeDamage(ball);
                }
            }
        }
    }

    public void DeleteEnemyFromList(Enemy deadEnemy)
    {
        EnemyList.Remove(deadEnemy);
    }
}
