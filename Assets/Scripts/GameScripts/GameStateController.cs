using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private float _startingGameHazzard;  /*количество заражения при котором игра начинается*/

    private float _currentHazzard; /*текущее количество заражения*/ 
    private GameState _currentGameState;
    private GameState _previousGameState;

    private void Start()
    {
        _currentGameState = GameState.PrepareGame;
    }

    private void Update()
    {
        ChangingGameSate();

        if (_currentGameState == GameState.Playing)
        {
            //Активизация игрового процесса
        }

        if (_currentGameState == GameState.Win)
        {
            Winlevel();
        }

        if (_currentGameState == GameState.Defeat)
        {
            LooseLevel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentGameState != GameState.Pause)
            {
                _previousGameState = _currentGameState;
                _currentGameState= GameState.Pause;
                Time.timeScale = 0;
            }
            else
            {
                _currentGameState = _previousGameState;
                Time.timeScale = 1;
            }
        }
    }

    private void LooseLevel()
    {
        throw new NotImplementedException();
    }

    private void Winlevel()
    {
        throw new NotImplementedException();
    }

    private void ChangingGameSate()
    {
        if (_currentHazzard >= _startingGameHazzard && _currentGameState != GameState.Playing)
        {
            _currentGameState = GameState.Playing;
        }

        if (_currentHazzard == 0)
        {
            _currentGameState = GameState.Win;
        }

        if (_currentHazzard == 100)
        {
            _currentGameState = GameState.Defeat;
        }
    }
}

enum GameState
{
    PrepareGame,
    Pause,
    Playing,
    Win,
    Defeat
}
