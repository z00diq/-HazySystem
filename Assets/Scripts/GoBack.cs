using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GoBack : MonoBehaviour
{

    [SerializeField] private Transform _playerTrasform;
    [SerializeField] private float _timeToBackInSeconds = 2f;
    [SerializeField] private float _timeToNextCast = 10f;
    [SerializeField] private ChargeIcon _goBackChargeIcon;

    private Ball _ball;
    private float _timer;
    private PlayerMove _playerMove;
    void Start()
    {
        _ball = GetComponent<Ball>();
        _playerMove = FindAnyObjectByType<PlayerMove>();
        _timer = _timeToNextCast;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G) && _timer > _timeToNextCast)
        {
            StartCoroutine(Back(_timeToBackInSeconds, _playerTrasform.position + Vector3.up));
            _timer = 0;
        }
        if (_ball.GetState() == State.Active)
        {
            _timer += Time.deltaTime;
        }
        _goBackChargeIcon.SetChargeValue(_timer, _timeToNextCast);
    }

    private IEnumerator Back(float time, Vector2 targetPosition)
    {
        Vector3 startBallPosition = transform.position;
        float startTime = Time.realtimeSinceStartup; 
        float fraction = 0f;
        _playerMove.SetState(State.Idle);
        _ball.AttackType = AttackType.Special;
        _ball.DamageValue += 10000000;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(startBallPosition, targetPosition, fraction);
            yield return null;
        }
        _ball.ChangeStateToIdle();
        _ball.AttackType = AttackType.Default;
        _ball.DamageValue -= 10000000;
        _playerMove.SetState(State.Active);
    }
}
