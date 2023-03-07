using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBall : MonoBehaviour
{
    [SerializeField] private float _timeToNextCast;
    [SerializeField] private float _timeOfAction;
    [SerializeField] private float _scalePowerUpBall = 1.4f;
    [SerializeField] private float _increaseDamage;

    private Ball _ball;
    private float _timer;
    private Vector3 _oldScale;
    private void Start()
    {
        _ball = GetComponent<Ball>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _timer > _timeToNextCast)
        {
            PowerUp();
            _timer = 0;
        }
        _timer += Time.deltaTime;
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
