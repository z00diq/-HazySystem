using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public Action OnWin;
    public Action OnLoose;

    [SerializeField] private float _startingGameHazzard;  /*количество заражения при котором игра начинается*/
    [SerializeField] private GameObject _endLevelPanel;
    [SerializeField] private List<GameObject> _enabledSpells; /*список боступных спеллов для получения игроком*/
    private float _currentHazzard; /*текущее количество заражения*/ 
    private GameState _currentGameState;
    private GameState _previousGameState;

    private Level _currentLevel;

    [ContextMenu("SetHazzard")]
    public void SetHazzard()
    {
        _currentHazzard = 33;

    }
    [ContextMenu("DestroyHazzard")]
    public void DestroyHazzard()
    {
        _currentHazzard = 0;
    }

    public void Initlevel(Level level)
    {
        _currentLevel = Instantiate(level, transform.position, Quaternion.identity);
        _currentLevel.InitLevel(this);
        _currentGameState = GameState.PrepareGame;
    }

    public void AddNewEnabledSpell(GameObject spell)
    {
        if(!_enabledSpells.Contains(spell))
            _enabledSpells.Add(spell);
    }

    private void Update()
    {
        if (_currentGameState == GameState.Playing)
        {
            ChangingGameSate();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_currentGameState != GameState.Pause)
                {
                    _previousGameState = _currentGameState;
                    _currentGameState = GameState.Pause;
                    Time.timeScale = 0;
                }
                else
                {
                    _currentGameState = _previousGameState;
                    Time.timeScale = 1;
                }
            }
            //Активизация игрового процесса
        }
        else
        {
            if (_currentHazzard >= _startingGameHazzard)
            {
                _currentGameState = GameState.Playing;
            }
        }

        if (_currentGameState == GameState.Win)
        {
            Winlevel();
        }

        if (_currentGameState == GameState.Defeat)
        {
            LooseLevel();
        }
    }

    private void LooseLevel()
    {
        OnLoose?.Invoke();
        _currentLevel.gameObject.SetActive(false);
    }

    private void Winlevel()
    {
        OnWin?.Invoke();
        _currentLevel.gameObject.SetActive(false);
    }


    private void ChangingGameSate()
    {
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
