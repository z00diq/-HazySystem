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
    [SerializeField] private float _slowBallDuration;
    private float _slowBallPeriod;

    private void Awake()
    {
        _enemyManager = _enemy.EnemyManager;
    }

    public void Initialize(bool canSlowBall, float SlowBallCooldown)
    {
        _canSlowBall = canSlowBall;
        _slowBallPeriod= SlowBallCooldown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Ball>() is Ball ball)
        {
            if (_canSlowBall && ball.AttackType == AttackType.Default)
            {
                StartCoroutine(SlowBall(ball));
                ball.StartCuroutineSlowBallForTime(_slowBallDuration);
            }
        }
    }

    public IEnumerator SlowBall(Ball ball)
    {
        _canSlowBall = false;

        yield return new WaitForSeconds(_slowBallPeriod);

        _canSlowBall = true;
    }
}
