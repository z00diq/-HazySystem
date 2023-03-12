using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timer;
    
    private Image _background;
    private void Awake()
    {
        _background = GetComponent<Image>();
    }
    public void SetChargeValue(float currentCharge, float maxCharge)
    {
        //_background.fillAmount = currentCharge / maxCharge;
        if (currentCharge < maxCharge)
        {
            _timer.enabled = true;
            _timer.text = Mathf.Ceil(maxCharge - currentCharge).ToString();
            _background.color = new Color(1,1,1,0.6f);
        }
        else
        {
            _timer.enabled = false;
            _background.color = new Color(1, 1, 1, 1);
        }
    }
    
} 
