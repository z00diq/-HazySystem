using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    public int EnemyCount;
    private List<Enemy> EnemyList = new List<Enemy>();

    public void Infestation()
    {
        Reproduce(transform.position);
    }

    public void Reproduce(Vector3 position)
    {
        // TODO: re-checking before creation
        if (Physics.CheckBox(position, new Vector3(0.06f, 0.06f, 0.05f), Quaternion.identity))
        {
            //Debug.Log($"рш еаюмскяъ, рср еярэ назейр {position}");
            return;
        }

        Enemy newCells = Instantiate(_enemyPrefab, position, Quaternion.identity);
        newCells.EnemyManager = this;
        newCells.transform.parent = transform;
        newCells.gameObject.name = "Enemy" + EnemyCount;

        EnemyCount++;

        EnemyList.Add(newCells);
    }
}
