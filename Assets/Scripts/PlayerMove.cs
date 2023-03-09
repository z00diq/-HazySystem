using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum DirectionOfMovement
    {
        Horizontal, 
        Vertical
    }
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _playerSpeed;

    private State _state = State.Active;
    private DirectionOfMovement _directionOfMovement = DirectionOfMovement.Horizontal;
    void Update()
    {
        if (_state == State.Idle)
            return;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (_directionOfMovement == DirectionOfMovement.Horizontal)
                _playerRigidbody.velocity = Vector3.left * _playerSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (_directionOfMovement == DirectionOfMovement.Horizontal)
                _playerRigidbody.velocity = Vector3.right * _playerSpeed;
        }
        else
        {
            _playerRigidbody.velocity = Vector3.zero;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (_state == State.Idle) { return; }
        Debug.Log(collision.contactCount);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Debug.Log(collision.contacts[i].point);
        }
        if (collision.contactCount == 2 && collision.gameObject.layer == 6)
        {
            
            //Debug.Log("2 Точка");
            _directionOfMovement = DirectionOfMovement.Vertical;
            Vector3 contactPointXY = collision.contacts[0].point - Vector3.forward * collision.contacts[0].point.z;
            if (contactPointXY.x > transform.position.x)
            {
                //Debug.Log("contactpoint");
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    Ray rayDown = new Ray(transform.position, Vector3.down);
                    if (Physics.Raycast(rayDown.origin, rayDown.direction, out RaycastHit hitInfo))
                    {
                        _playerRigidbody.velocity = (hitInfo.point - contactPointXY).normalized * _playerSpeed;
                    }
                    else
                    {
                        Ray rayRight = new Ray(transform.position, Vector3.right);
                        if (Physics.Raycast(rayRight.origin, rayRight.direction, out hitInfo))
                        {
                            _playerRigidbody.velocity = (contactPointXY - hitInfo.point).normalized * _playerSpeed;
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    Ray ray = new Ray(transform.position, Vector3.right);
                    if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
                    {
                        _playerRigidbody.velocity = (hitInfo.point - contactPointXY).normalized * _playerSpeed;
                        //Debug.Log("hitInfo.point " + hitInfo.point);
                        //Debug.Log("contactPointXY " + contactPointXY);
                        //Debug.Log("hitInfo.point - contactPointXY " + (hitInfo.point - contactPointXY));
                        
                    }
                    else
                    {
                        Ray rayDown = new Ray(transform.position, Vector3.down);
                        if (Physics.Raycast(rayDown.origin, rayDown.direction, out hitInfo))
                        {
                            _playerRigidbody.velocity = (contactPointXY - hitInfo.point).normalized * _playerSpeed;
                        }
                    }
                    
                }
            }


            //
            if (contactPointXY.x < transform.position.x)
            {
                //Debug.Log("contactpoint");
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    Ray rayRight = new Ray(transform.position, Vector3.left);
                    if (Physics.Raycast(rayRight.origin, rayRight.direction, out RaycastHit hitInfo))
                    {
                        _playerRigidbody.velocity = (hitInfo.point - contactPointXY).normalized * _playerSpeed;
                    }
                    else
                    {
                        Ray rayDown = new Ray(transform.position, Vector3.down);
                        if (Physics.Raycast(rayDown.origin, rayDown.direction, out  hitInfo))
                        {
                            _playerRigidbody.velocity = (contactPointXY - hitInfo.point).normalized * _playerSpeed;
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    Ray rayDown = new Ray(transform.position, Vector3.down);
                    if (Physics.Raycast(rayDown.origin, rayDown.direction, out RaycastHit hitInfo))
                    {
                        _playerRigidbody.velocity = (hitInfo.point - contactPointXY).normalized * _playerSpeed;
                    }
                    
                    else
                    {
                        Ray ray = new Ray(transform.position, Vector3.left);
                        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                        {
                            _playerRigidbody.velocity = (contactPointXY - hitInfo.point).normalized * _playerSpeed;
                            //Debug.Log("hitInfo.point " + hitInfo.point);
                            //Debug.Log("contactPointXY " + contactPointXY);
                            //Debug.Log("hitInfo.point - contactPointXY " + (hitInfo.point - contactPointXY));
                        }
                    }

                }
            }
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        _directionOfMovement = DirectionOfMovement.Horizontal;
    }

    public void SetState(State state)
    {
        _state = state;
    }
}
