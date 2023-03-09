using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyReproduce : MonoBehaviour
{
    // DEBUG
    [SerializeField] private bool _canShowRays;

    [SerializeField] private EnemyManager _enemyManager;

    [SerializeField] private float _reproductionPeriod;

    private Coroutine ReproduceCoroutine;

    [SerializeField] private List<Vector3> _rayDirections = new List<Vector3>();
    
    public void Initialize(EnemyManager EnemyManager, bool haveFastReproduction)
    {
        _enemyManager = EnemyManager;
        SetupReproductionPeriod(haveFastReproduction);

        _enemyManager.ReproduceRuleChanged.AddListener(OnReproduceChanged);
        StartReproduceCoroutine();
    }

    private void OnDisable()
    {
        _enemyManager.ReproduceRuleChanged.RemoveListener(OnReproduceChanged);
    }

    private void StartReproduceCoroutine()
    {
        ReproduceCoroutine = StartCoroutine(ReproductionTimer(_reproductionPeriod));
    }

    private void StopReproduceCoroutine()
    {
        StopCoroutine(ReproduceCoroutine);
        ReproduceCoroutine = null;
    }

    public IEnumerator ReproductionTimer(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        while (true)
        {
            ShowRays();
            yield return wait;
            ReproductionProcess();
        }
    }

    private void ReproductionProcess()
    {
        Vector3 newCellPosition = GetPositionForNewCells();
        if (newCellPosition != new Vector3(100, 100, 100))
        {
            _enemyManager.Reproduce(newCellPosition);
        }
    }

    private void OnReproduceChanged(bool value)
    {
        if (value == true)
        {
            if (ReproduceCoroutine == null)
            {
                StartReproduceCoroutine();
            }
        }
        else if(ReproduceCoroutine != null)
        {
            StopReproduceCoroutine();
        }
    }

    private Vector3 GetPositionForNewCells()
    {
        Vector3 vectorToNewCells = Vector3.zero;

        for (int i = 0; i < _rayDirections.Count; i++)
        {
            if(Physics.CheckBox(transform.position + _rayDirections[i] * _enemyManager.SpawnDistance, new Vector3(0.06f, 0.06f, 0.05f), Quaternion.identity))
            {
                continue;
            }
            else
            {
                vectorToNewCells = _rayDirections[i] * _enemyManager.SpawnDistance;
                return transform.position + vectorToNewCells;
            }

        }

        return new Vector3(100, 100, 100);
    }

    public void SetupReproductionPeriod(bool isActive)
    {
        float offset = Random.Range(-0.5f, 1f);

        _reproductionPeriod = isActive ? _enemyManager.ReproductionPeriod / 2 + offset : _enemyManager.ReproductionPeriod + offset;
        _reproductionPeriod = Mathf.Clamp(_reproductionPeriod, 1, float.PositiveInfinity);
    }

    //DEBUG
    private void ShowRays()
    {
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            Debug.DrawRay(transform.position, _rayDirections[i] * _enemyManager.SpawnDistance);
        }
    }
}
