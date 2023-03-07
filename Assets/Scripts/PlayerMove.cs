using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _playerSpeed;

    private State _state = State.Active;

    void Update()
    {
        if (_state == State.Idle)
            return;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _playerRigidbody.velocity = Vector3.left * _playerSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _playerRigidbody.velocity = Vector3.right * _playerSpeed;
        }
        else
        {
            _playerRigidbody.velocity = Vector3.zero;
        }
    }

    public void SetState(State state)
    {
        _state = state;
    }
}
