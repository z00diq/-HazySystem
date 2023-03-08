using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ememy : MonoBehaviour
{
    [SerializeField] private float _health = 1;
    //[SerializeField] private float _reproductionPeriod = 5;

    private void OnTriggerEnter(Collider other)
    {
        // if collision with ball - take damage
        if (other.GetComponent<Ball>())
        {
            _health -= other.GetComponent<Ball>().DamageValue;

            if (CheckDeath())
            {
                //TODO: add death animation
                Destroy(gameObject);
                Debug.Log("Enemy dead");
            }
        }
    }

    private bool CheckDeath()
    {
        return _health <= 0 ? true : false;
    }
}
