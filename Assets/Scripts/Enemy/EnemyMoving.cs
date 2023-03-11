using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;

    [SerializeField] private bool _canMoving;
    [SerializeField] public bool IsMoving;

    [SerializeField] private float _movingPeriod;
    [SerializeField] private float _movingDuration;

    private Coroutine MovingCoroutine;

    public void Initialize(bool canMoving, float movingCooldown)
    {
        _canMoving = canMoving;
        _movingPeriod = movingCooldown;
    }

    private void Start()
    {
        if (_enemyManager == null)
        {
            _enemyManager = FindObjectOfType<EnemyManager>();
        }

        _enemyManager.MovingRuleChanged.AddListener(OnMovingChange);
        if (_canMoving == true)
        {
            StartMovingCoroutime();
        }
    }

    private void OnDisable()
    {
        _enemyManager.MovingRuleChanged.RemoveListener(OnMovingChange);
    }

    private void StartMovingCoroutime()
    {
        MovingCoroutine = StartCoroutine(MovingTimer(_movingPeriod));
    }

    private void StopMovingCoroutine()
    {
        StopCoroutine(MovingCoroutine);
        MovingCoroutine = null;
    }

    public IEnumerator MovingTimer(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        while (true)
        {
            yield return wait;
            MovingProcess();
        }
    }

    private void MovingProcess()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * (_enemyManager.SpawnDistance * 2));
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<EnemyMoving>() is EnemyMoving target)
            {
                if (target != this)
                {
                    if ((!target.IsMoving && !this.IsMoving) && (target._canMoving && this._canMoving))
                    {
                        Vector3 pos1 = this.transform.position;
                        Vector3 pos2 = target.transform.position;

                        StartCoroutine(ChangePosition(pos1, this, pos2, target));
                        return;
                    }
                }
            }
        }
    }

    private void OnMovingChange(bool value)
    {
        if (value == true && _canMoving == true)
        {
            if (MovingCoroutine == null)
            {
                StartMovingCoroutime();
            }
        }
        else if (MovingCoroutine != null)
        {
            StopMovingCoroutine();
        }
    }

    private IEnumerator ChangePosition(Vector3 pos1, EnemyMoving enemy1, Vector3 pos2, EnemyMoving enemy2)
    {
        IsMoving = true;
        enemy2.GetComponent<EnemyMoving>().IsMoving = true;
        float timer = 0;
        while (timer < _movingDuration)
        {
            timer += Time.deltaTime;
            enemy1.transform.position = Vector3.Lerp(enemy1.transform.position, pos2, (timer / _movingDuration));
            enemy2.transform.position = Vector3.Lerp(enemy2.transform.position, pos1, (timer / _movingDuration));
            yield return null;
        }

        IsMoving = false;
        enemy2.GetComponent<EnemyMoving>().IsMoving = false;
    }

}
