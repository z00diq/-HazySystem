using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameStateController _gameStateController;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loosePanel;
    [SerializeField] private GameObject _choosingLevelPanel;


    private void OnEnable()
    {
        _gameStateController.OnWin += GameStateController_OnWin;
        _gameStateController.OnLoose += GameStateController_OnLoose;
    }


    private void OnDisable()
    {
        _gameStateController.OnWin -= GameStateController_OnWin;
        _gameStateController.OnLoose -= GameStateController_OnLoose;
    }
    private void GameStateController_OnLoose()
    {
        _winPanel.SetActive(true); 
    }

    private void GameStateController_OnWin()
    {
        _loosePanel.SetActive(true);
    }
}
