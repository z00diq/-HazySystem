using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGroup : MonoBehaviour
{
    private void OnEnable()
    {
        GameStateController.OnLoose += DeactivateChargeIcon;
        GameStateController.OnWin += DeactivateChargeIcon;
    }

    private void DeactivateChargeIcon()
    {
        for (int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameStateController.OnLoose -= DeactivateChargeIcon;
        GameStateController.OnWin -= DeactivateChargeIcon;
    }
}
