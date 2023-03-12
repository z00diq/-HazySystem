using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HazzardPanel : MonoBehaviour
{
    [SerializeField] private Image _hazzardImage;
    [SerializeField] private TMP_Text _hazzardText;

    private float _maxFillValue;

    private void OnEnable()
    {
        GameStateController.OnWin += OnEnablePanel;
        GameStateController.OnLoose+= OnEnablePanel;
        GameStateController.OnGameStart += OnGameStart;
        GameStateController.OnEnemyCountChanged += OnEnemyCountChanged;
    }

    private void OnEnablePanel()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameStateController.OnGameStart -= OnGameStart;
        GameStateController.OnEnemyCountChanged -= OnEnemyCountChanged;
    }

    private void Start()
    {
        _hazzardImage.fillAmount = 0;
        _hazzardText.gameObject.SetActive(false);
    }

    private void OnEnemyCountChanged(float value)
    {
        if (_hazzardText.IsActive() == false) 
            _hazzardText.gameObject.SetActive(true);

        _hazzardText.text = $"{value}/{_maxFillValue}";
        _hazzardImage.fillAmount = value / _maxFillValue;
    }

    private void OnGameStart(float value)
    {
        _maxFillValue=value;
    }

    
}
