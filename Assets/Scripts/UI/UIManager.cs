using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Action OnUnPauseButtonClick;
    public static Action OnToLevelsButtonClick;
    public static Action OnToMainMenyButtonClick;

    [SerializeField] private GameStateController _gameStateController;
    [SerializeField] private GameObject _gameStartPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loosePanel;
    [SerializeField] private GameObject _choosingLevelPanel;
    [SerializeField] private GameObject _pausePanel;

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

    public void UnPauseButton_OnButtonClick()
    {
        OnUnPauseButtonClick?.Invoke();
    }

    public void ToLevelsButton_OnButtonClick()
    {
        OnToLevelsButtonClick?.Invoke();
        _choosingLevelPanel.SetActive(true);
    }

    public void ToMainMenu_OnButtonClick()
    {
        OnToMainMenyButtonClick?.Invoke();
        _gameStartPanel.SetActive(true);
    }

    private void OnEnable()
    {
        GameStateController.OnWin += GameStateController_OnWin;
        GameStateController.OnLoose += GameStateController_OnLoose;
        GameStateController.OnPause += GameStateController_OnPause;
        GameStateController.OnUnPause += GameStateController_OnUnPause;
    }
    private void OnDisable()
    {
        GameStateController.OnWin -= GameStateController_OnWin;
        GameStateController.OnLoose -= GameStateController_OnLoose;
        GameStateController.OnPause -= GameStateController_OnPause;
        GameStateController.OnUnPause -= GameStateController_OnUnPause;
    }

    private void GameStateController_OnUnPause()
    {
        _pausePanel.SetActive(false);
    }

    private void GameStateController_OnPause()
    {
        _pausePanel.SetActive(true);
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
