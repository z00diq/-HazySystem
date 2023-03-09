using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemiesGlobalValues _enemiesValues;

    [SerializeField] private Enemy _enemyPrefab;

    public int EnemyCount;
    private List<Enemy> EnemyList = new List<Enemy>();

    public float ReproductionPeriod;
    public float AbilityPeriod;

    //public bool CanReproduce;

    private void Awake()
    {
        ReproductionPeriod = _enemiesValues.ReproductionPeriodBase;
        AbilityPeriod = _enemiesValues.AbilityPeriod;

        Reproduce(transform.position);
    }

    public void Reproduce(Vector3 position)
    {
        if(CheckEmptyPlace(position))
        {
            Enemy newCells = Instantiate(_enemyPrefab, position, Quaternion.identity);
            newCells.Initialize(this,
                CanHaveAbilities(),     // fast reproduction
                SetupMaxHealth(),       // max health
                CanHaveAbilities(),     // invul
                CanHaveAbilities(),     // can heal himself
                CanHaveAbilities(),     // can heal another
                CanHaveAbilities(),     // can transfer damage
                CanHaveAbilities(),     // can change position
                CanHaveAbilities()      // can slow ball
                );    

            newCells.transform.parent = transform;
            newCells.gameObject.name = "Enemy" + EnemyCount;
            EnemyCount++;

            EnemyList.Add(newCells);
        }
    }

    private bool CheckEmptyPlace(Vector3 position)
    {
        if (Physics.CheckBox(position, new Vector3(0.06f, 0.06f, 0.05f), Quaternion.identity))
        {
            //Debug.Log($"�� ��������, ��� ���� ������ {position}");
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
}