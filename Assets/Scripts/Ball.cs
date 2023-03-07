using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public enum BallState
{
    Idle,
    Active
}
public class Ball : MonoBehaviour
{
    [SerializeField] public float _damageValue;
    [SerializeField] private Rigidbody _ballRigidbody;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _ballSpeed = 10f;
    [SerializeField] private float _lowerLimitOfTheDirectionOfMovement = 0.5f;
    [SerializeField] private LineRenderer _lineRenderer;

    private Vector3 _directionOfMovement;
    private BallState _currentBallState;


    void Update()
    {
        if (_currentBallState == BallState.Idle)
        {
            _directionOfMovement = SetDirectionOfMovement();
            if (Input.GetMouseButton(0) )
            {
                Shot();
            }
        }

        if (transform.position.y < 0)
        {
            Debug.Log("GameOver");
        }    
    }

    private void Shot()
    {
        _ballRigidbody.isKinematic = false;
        _ballRigidbody.velocity = _directionOfMovement * _ballSpeed;
        _currentBallState = BallState.Active;
        _lineRenderer.enabled = false;
    }

    private Vector3 SetDirectionOfMovement()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(_playerCamera.transform.position, ray.direction * 15f, Color.yellow);

        Plane plane = new Plane(-Vector3.forward, Vector3.zero);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 point = transform.InverseTransformPoint(ray.GetPoint(distance));

        if (point.y < transform.position.y + _lowerLimitOfTheDirectionOfMovement)
        {
            point.y = transform.position.y + _lowerLimitOfTheDirectionOfMovement;
        }
        DrawLine(Vector3.zero, point);
        return point.normalized;
    }
    private void DrawLine(Vector3 start, Vector3 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, end);
    }
}
