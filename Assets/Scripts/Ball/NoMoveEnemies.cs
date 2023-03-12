using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoveEnemies : Ability
{
    [SerializeField] private float _timeOfAction = 3f;

    private EnemyManager _enemyManager;
    protected override void Start()
    {
        base.Start();
        _enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _timer > _timeToNextCast)
        {
            _enemyManager.StopEnemyMoving(_timeOfAction);
            _timer = 0;
        }
        _timer += Time.deltaTime;
        ChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }
}
