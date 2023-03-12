using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public enum State
{
    Inactive,
    Active
}
public enum AttackType
{
    Default,
    Special
}
public class Ball : MonoBehaviour
{
    public float DamageValue;
    public AttackType AttackType { get; set; }

    public float _ballSpeed;
    [SerializeField] private Rigidbody _ballRigidbody;
    [SerializeField] private float _lowerLimitOfTheDirectionOfMovement = 0.5f;
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] public float _ballSpeedFull = 15f;
    [SerializeField] public float _ballSpeedSlow = 8f;


    private Transform _playerTrnasform;
    private Camera _playerCamera;
    private Vector3 _directionOfMovement;
    private State _currentBallState;
    private void Start()
    {
        _ballSpeed = _ballSpeedFull;
        _playerCamera = Camera.main;
        _playerTrnasform = FindObjectOfType<PlayerMove>().transform;
    }

    void Update()
    {
        if (_currentBallState == State.Inactive)
        {
            transform.position = _playerTrnasform.position + Vector3.up * 0.5f * (transform.localScale.y + _playerTrnasform.localScale.y);
            _directionOfMovement = SetDirectionOfMovement();
            if (Input.GetMouseButtonDown(0) )
            {
                Shot();
            }
        }
        

        if (transform.position.y < 0)
        {
            //Debug.Log("GameOver");
        }
        //Debug.Log(_ballRigidbody.velocity.magnitude);
    }
    
    private void OnCollisionExit(Collision collision)
    {
        
        //исправление для того, чтобы не было постоянного горизонтального или вертикального движения
        if (_currentBallState == State.Active)
        {
            ChangeVelocityMagnitude(_ballSpeed);
            if (_ballRigidbody.velocity.x <= 0.01f)
                _ballRigidbody.velocity = _ballRigidbody.velocity + 20f * Vector3.right;
            if (_ballRigidbody.velocity.y <= 0.01f)
                _ballRigidbody.velocity = _ballRigidbody.velocity + 20f * Vector3.up;
        }
        
    }

    private void Shot()
    {
        _ballRigidbody.velocity = _directionOfMovement * _ballSpeed;
        _currentBallState = State.Active;
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

        if (point.y < _lowerLimitOfTheDirectionOfMovement)
        {
            point.y = _lowerLimitOfTheDirectionOfMovement;
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
    /*public void ChangeStateToInactive()
    {
        _lineRenderer.enabled = true;
        _currentBallState = State.Inactive;
    }*/
    

    public State GetState() { return _currentBallState; }
    public void SetState(State state)
    { 
        _currentBallState = state;
        if (_currentBallState == State.Active)
            _lineRenderer.enabled = false;
        else
            _lineRenderer.enabled = true;
    }

    public void StartCuroutineSlowBallForTime(float time)
    {
        StartCoroutine(SlowBallForTime(time));
    }
    private IEnumerator SlowBallForTime(float time)
    {
        _ballSpeed = _ballSpeedSlow;
        yield return new WaitForSeconds(time);
        _ballSpeed = _ballSpeedFull;
        ChangeVelocityMagnitude(_ballSpeed);
    }
    
    private void ChangeVelocityMagnitude(float velocity)
    {
        _ballRigidbody.velocity = _ballRigidbody.velocity.normalized * _ballSpeed;
    }
}
