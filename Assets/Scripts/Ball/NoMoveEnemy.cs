using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoveEnemy : MonoBehaviour
{
    [SerializeField] private float _timeToNextCast = 7f;
    [SerializeField] private float _timeOfAction = 3f;
    [SerializeField] private ChargeIcon _noMoveEnemyChargeIcon;

    private EnemyManager _enemyManager;
    private float _timer;
    void Start()
    {
        _enemyManager = FindAnyObjectByType<EnemyManager>();
        _timer = _timeToNextCast;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && _timer > _timeToNextCast)
        {
            _enemyManager.StopEnemyMoving(_timeOfAction);
            _timer = 0;
        }
        _timer += Time.deltaTime;
        _noMoveEnemyChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }
}
