using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemiesGlobalValues _enemiesValues;

    [SerializeField] private GameObject _enemyPrefab;

    public float SpawnDistance = 0.2f;

    public int EnemyCount;
    private List<Enemy> EnemyList = new List<Enemy>();

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
        CanReproduce = true;
        ReproductionPeriod = _enemiesValues.ReproductionPeriodBase;
        AbilityPeriod = _enemiesValues.AbilityPeriod;

        Reproduce(transform.position);
    }

    public void Reproduce(Vector3 position)
    {
        if(CheckEmptyPlace(position))
        {
            GameObject newCells = Instantiate(_enemyPrefab, position, Quaternion.identity);
            Enemy newCellsEnemy = newCells.GetComponent<Enemy>();
            EnemyReproduce newCellsEnemyReproduce = newCells.GetComponent<EnemyReproduce>();

            newCellsEnemy.Initialize(this,
                SetupMaxHealth(),                       // max health
                CanHaveAbilities(),                     // invul
                CanHaveAbilities(),                     // can heal himself
                CanHaveAbilities(),                     // can heal another
                CanHaveAbilities(),                     // can transfer damage
                CanHaveAbilities(),                     // can change position
                CanHaveAbilities()                      // can slow ball
                );
            newCellsEnemyReproduce.Initialize(this,
                CanHaveAbilities()                      // fast reproduction
                );

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
            //Debug.Log($"“€ ≈¡¿Õ”À—ﬂ, “”“ ≈—“‹ Œ¡⁄≈ “ {position}");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CanHaveAbilities()
    {
        return (int)Random.Range(0, 100) <= _enemiesValues.ChanceGetAbility ? true: false;
    }

    private float SetupMaxHealth()
    {
        return (int)Random.Range(0, 100) <= _enemiesValues.ChanceGetAbility ? _enemiesValues.MaximumHealthForCells : _enemiesValues.MinimumHealthForCells;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Change Reproduction");
            CanReproduce = !CanReproduce;
        }
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

}
