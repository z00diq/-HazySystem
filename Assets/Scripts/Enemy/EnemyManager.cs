using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private LayerMask _layerMask;

    public int EnemyCount;
    private List<Enemy> EnemyList = new List<Enemy>();

    public void Infestation()
    {
        Reproduce(transform.position);
    }

    public void ClearEnemies()
    {
        if (EnemyList.Count > 0)
        {
            foreach (Enemy enemy in EnemyList)
            {
                Destroy(enemy.gameObject);
            }

            EnemyList.Clear();
        }
    }

    public void Reproduce(Vector3 position)
    {
        // TODO: re-checking before creation
        if (Physics.CheckBox(position, new Vector3(0.06f, 0.06f, 0.05f), Quaternion.identity, _layerMask))
        {
            Debug.Log($"рш еаюмскяъ, рср еярэ назейр {position}");
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
