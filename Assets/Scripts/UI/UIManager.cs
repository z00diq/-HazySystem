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
    [SerializeField] private GameObject _startingPanel;

    public void RestartButton_OnButtonClick()
    {
        _gameStateController.InitLevel();
    }

    public void StartGameButton_OnButtonClick()
    {
        _choosingLevelPanel.SetActive(true);
    }

    public void QuitGameButton_OnButtonClick()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        GameStateController.OnWin += GameStateController_OnWin;
        GameStateController.OnLoose += GameStateController_OnLoose;
    }

    private void OnDisable()
    {
        GameStateController.OnWin -= GameStateController_OnWin;
        GameStateController.OnLoose -= GameStateController_OnLoose;
    }
    private void GameStateController_OnLoose()
    {
        _loosePanel.SetActive(true); 
    }

    private void GameStateController_OnWin()
    {
        _winPanel.SetActive(true);
    }
}
