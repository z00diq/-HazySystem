using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesLevelRules : MonoBehaviour
{
    [SerializeField] public float SpawnDistance;
    [SerializeField] public float ReproductionDelay;
    [SerializeField] public float ReproductionPeriodBase;
    [SerializeField] public float AbilityPeriod;

    [SerializeField] public float MinimumHealthForCells;
    [SerializeField] public float MaximumHealthForCells;

    [Range(0, 100)][SerializeField] public int ChanceGetAbility;

    [SerializeField] public bool CanHaveFastReproduction;
    [SerializeField] public bool CanHaveBiggerHealth;
    [SerializeField] public bool CanDefence;
    [SerializeField] public bool CanSlowBall;
    [SerializeField] public bool CanHealHimself;
    [SerializeField] public bool CanHealAnother;
    [SerializeField] public bool CanTransferDamage;
    [SerializeField] public bool CanChangePosition;

    [SerializeField] public int EnemyMaximumAbilities;
}
