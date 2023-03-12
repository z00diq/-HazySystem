using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopReproduction : Ability
{
    [SerializeField] private float _timeOfAction = 6f;
    

    private EnemyManager _enemyManager;
    protected override void Start()
    {
        base.Start();
        _enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _timer > _timeToNextCast)
        {
            StartCoroutine(_enemyManager.StopReproduce(_timeOfAction));
            _timer = 0;
        }
        _timer += Time.deltaTime;
        ChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }
}
