using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeIcon : MonoBehaviour
{
    private Image _background;
    private void Awake()
    {
        _background = GetComponent<Image>();
    }
    public void SetChargeValue(float currentCharge, float maxCharge)
    {
        _background.fillAmount = currentCharge / maxCharge;
    }
    
} 
