using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _playerColliderTransform;
    [SerializeField] private float _jumpHieght = 2f;
    [SerializeField] private float _dropHieght = 5f;
    [SerializeField] private Transform _leftLimitation;
    [SerializeField] private Transform _rightLimitation;


    private State _state = State.Active;
    private Ball _currentBall;

    private void Start()
    {
        GameStateController.OnDestroyPlayer += OnDestroyPlayer;
        _leftLimitation.parent = null;
        _rightLimitation.parent = null;
        SpawnBall();
    }

    private void OnDestroyPlayer()
    {
        if (_currentBall != null)
        {
            Destroy(_currentBall.gameObject);
            _currentBall = null;
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if (_state == State.Inactive) return;
        if (Input.GetAxis("Horizontal") < 0 && transform.position.x < _leftLimitation.position.x) return;
        if (Input.GetAxis("Horizontal") > 0 && transform.position.x > _rightLimitation.position.x) return;

        transform.position += Vector3.right * Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        
        if (Physics.BoxCast(transform.position + new Vector3(0f, _jumpHieght, 0f),
            _playerColliderTransform.localScale * 0.5f,
            Vector3.down,
            out RaycastHit hit,
            Quaternion.identity,
            10f,
            _layerMask))
        {
            if (hit.distance < _dropHieght)
            {
                transform.position += hit.distance * Vector3.down + new Vector3(0f, _jumpHieght, 0f);
            }            
        }
    }


    private void OnDestroy()
    {
        GameStateController.OnDestroyPlayer -= OnDestroyPlayer;
    }
    private void SpawnBall()
    {
        _currentBall = Instantiate(_ballPrefab);
    }

    public void SetState(State state)
    {
        _state = state;
    }

    public void SetLimites(Transform leftLim,Transform rightLim)
    {
        _leftLimitation = leftLim;
        _rightLimitation = rightLim;
    }
}
