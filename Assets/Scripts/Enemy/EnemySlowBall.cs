using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemySlowBall : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private EnemyManager _enemyManager;

    [SerializeField] private bool _canSlowBall;
    [SerializeField] private float _slowBallDivider;
    [SerializeField] private float _slowBallDuration;
    [SerializeField] private float _slowBallCooldown;

    private void Awake()
    {
        _enemyManager = _enemy.EnemyManager;
    }

    public void Initialize(bool canSlowBall)
    {
        _canSlowBall= canSlowBall;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Ball>() is Ball ball)
        {
            if (_canSlowBall)
            {
                StartCoroutine(SlowBall(ball));
            }
        }
    }

    public IEnumerator SlowBall(Ball ball)
    {
        StartCoroutine(ball.SlowBallForTime(_slowBallDivider, _slowBallDuration));
        _canSlowBall = false;

        yield return new WaitForSeconds(_slowBallCooldown);

        _canSlowBall = true;
    }
}
