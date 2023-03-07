using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _playerSpeed;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            _playerRigidbody.velocity = Vector3.left * _playerSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _playerRigidbody.velocity = Vector3.right * _playerSpeed;
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            _playerRigidbody.velocity = Vector3.zero;
        }
    }
}
