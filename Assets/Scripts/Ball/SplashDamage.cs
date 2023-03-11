using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : Ability
{
    
    [SerializeField] private float _radiusOfCast = 4;

    private Ball _ball;
    protected override void Start()
    {
        base.Start();
        _ball = GetComponent<Ball>();
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
        ChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }
    private void SplashDamageCast(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.TakeDamage(_ball);
            }
        }
    }
}