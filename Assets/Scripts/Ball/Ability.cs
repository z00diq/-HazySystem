using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [field: SerializeField] public ChargeIcon ChargeIcon{ get; set; }
    [SerializeField] protected float _timeToNextCast = 15f;
}
