using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBall : Ability
{
    [SerializeField] private float _timeOfAction = 7f;
    [SerializeField] private float _scalePowerUpBall = 1.4f;
    [SerializeField] private float _increaseDamage;

    private Ball _ball;
    private Vector3 _oldScale;
    protected override void Start()
    {
        base.Start();
        _ball = GetComponent<Ball>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _timer > _timeToNextCast)
        {
            PowerUp();
            _timer = 0;
        }
        if (_ball.GetState() == State.Active)
        {
            _timer += Time.deltaTime;
        }
        ChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }

    private void PowerUp()
    {
        _oldScale = transform.localScale;
        transform.localScale = new Vector3(_scalePowerUpBall, _scalePowerUpBall, _scalePowerUpBall);
        _ball.DamageValue += _increaseDamage;
        Invoke("PowerDown", _timeOfAction);
    }
    private void PowerDown()
    {
        transform.localScale = _oldScale;
        _ball.DamageValue -= _increaseDamage;
    }
}
