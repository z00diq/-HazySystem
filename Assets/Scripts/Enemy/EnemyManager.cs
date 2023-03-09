using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    public int EnemyCount;
    private List<Enemy> EnemyList = new List<Enemy>();

    public float ReproductionPeriodBase;

    //public bool CanReproduce;

    private void Awake()
    {
        Reproduce(transform.position);
    }

    public void Reproduce(Vector3 position)
    {
        if(CheckEmptyPlace(position))
        {
            Enemy newCells = Instantiate(_enemyPrefab, position, Quaternion.identity);
            newCells.Initialize(this, false, 1, false, false, false, false, false);

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
            //Debug.Log($"“€ ≈¡¿Õ”À—ﬂ, “”“ ≈—“‹ Œ¡⁄≈ “ {position}");
            return false;
        }
        else
        {
            return true;
        }
    }
}
