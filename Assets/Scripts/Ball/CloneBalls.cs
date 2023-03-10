using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBalls : Ability
{
    [SerializeField] private int _numberOfClones = 2;
    [SerializeField] private int _countOfCollisionsToDie = 5;
    [SerializeField] private Ball _cloneBallPrefab;

    private float _timer;
    private void Start()
    {
        _timer = _timeToNextCast;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && _timer > _timeToNextCast)
        {
            Reproduction(_numberOfClones);
            _timer = 0;
        }
        _timer += Time.deltaTime;
        ChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }
    private void Reproduction(int numberOfClones)
    {
        for (int i = 0; i < numberOfClones; i++)
        {
            Ball ball = Instantiate(_cloneBallPrefab, transform.position, Quaternion.identity);
            ball.SetState(State.Active);
            ball.GetComponent<DestroyOnCollisions>().SetHealth(_countOfCollisionsToDie);
        }
    }
}
