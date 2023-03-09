using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGlobalValues : MonoBehaviour
{
    [SerializeField] public float ReproductionPeriodBase;
    [SerializeField] public float AbilityPeriod;

    [SerializeField] public float MinimumHealthForCells;
    [SerializeField] public float MaximumHealthForCells;

    [Range(0, 100)][SerializeField] public int ChanceGetAbility;
}
