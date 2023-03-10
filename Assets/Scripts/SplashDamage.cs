using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    [SerializeField] private float _timeToNextCast;
    [SerializeField] private float _radiusOfCast = 4;
    [SerializeField] private ChargeIcon _powerUpBallChargeIcon;

    private Ball _ball;
    private float _timer;
    private void Start()
    {
        _ball = GetComponent<Ball>();
        _timer = _timeToNextCast;
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.S) && _timer > _timeToNextCast)
        {
            SplashDamageCast(_radiusOfCast);
            _timer = 0;
        }
        if (_ball.GetState() == State.Active)
        {
            _timer += Time.deltaTime;
        }
        _powerUpBallChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }
    private void SplashDamageCast(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.TakeDamage(_ball.DamageValue, _ball.AttackType);
            }
        }
    }
}