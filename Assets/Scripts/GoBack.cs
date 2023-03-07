using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GoBack : MonoBehaviour
{

    [SerializeField] private UnityAction _goBack;
    [SerializeField] private Transform _playerTrasform;
    [SerializeField] private float _timeToBackInSeconds = 2f;
    [SerializeField] private float _timeToNextCast = 10f;

    private Ball _ball;
    private float _timer;
    private PlayerMove _playerMove;
    void Start()
    {
        _ball = GetComponent<Ball>();
        _playerMove = FindAnyObjectByType<PlayerMove>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G) && _timer > _timeToNextCast)
        {
            StartCoroutine(Back(_timeToBackInSeconds, _playerTrasform.position + Vector3.up));
            _timer = 0;
        }
        _timer += Time.deltaTime;
    }

    private IEnumerator Back(float time, Vector2 targetPosition)
    {
        Vector3 startBallPosition = transform.position;
        float startTime = Time.realtimeSinceStartup; 
        float fraction = 0f;
        _playerMove.SetState(State.Idle);
        _ball.DamageValue += 1000000f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(startBallPosition, targetPosition, fraction);
            yield return null;
        }
        _ball.ChangeStateToIdle();
        _ball.DamageValue -= 1000000f;
        _playerMove.SetState(State.Active);
    }
}
