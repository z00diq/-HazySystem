using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayEventManager : MonoBehaviour
{
    public static UnityEvent OnEnemyKilled = new UnityEvent();
    public static UnityEvent OnReceiveAbility = new UnityEvent();
    
    public static void SendEnemyKilled()
    {
        OnEnemyKilled.Invoke();
    }
    public static void SendReceiveAbility()
    {
        OnReceiveAbility.Invoke();
    }
}
