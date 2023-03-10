using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollisions : MonoBehaviour
{
    private int _health;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Ball>())
        {
            _health--;
            if (_health <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }

       
    }
    public void SetHealth(int health)
    {
        _health = health;
    }
}
