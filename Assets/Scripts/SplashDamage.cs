using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    [SerializeField] private float _timeToNextCast;
    [SerializeField] private float _timeOfAction;
    [SerializeField] private float _radiusOfCast;
    [SerializeField] private ChargeIcon _powerUpBallChargeIcon;

    private Ball _ball;
    // Start is called before the first frame update
    private void Start()
    {
        _ball = GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
       
        /*if (Input.GetKeyDown(KeyCode.Q) && _timer > _timeToNextCast)
        {
            PowerUp();
            _timer = 0;
        }
        if (_ball.GetState() == State.Active)
        {
            _timer += Time.deltaTime;
        }
        _powerUpBallChargeIcon.SetChargeValue(_timer, _timeToNextCast);*/
    }
    /*private void SplashDamage()
    {

    }*/
}
