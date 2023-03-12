using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static Action OnWin;
    public static Action OnLoose;
    public static Action OnPause;
    public static Action OnUnPause;
    public static Action OnDestroyPlayer;

    public Level CurrentLevel => _currentLevel;

    //public static Action OnGameStart;
    [SerializeField] private float _startingGameHazzard;  
    [SerializeField] private float _deathHazzardLevel;  
    [SerializeField] private List<GameObject> _enabledSpells;
    [SerializeField] private PlayerMove _platform;
    private float _currentHazzard; 
    private GameState _currentGameState;

    private Level _currentLevel;
    private bool isPause=false;

    public void InitLevel(Level level=null)
    {
        if(level != null)
        {
            _currentLevel = Instantiate(level, transform.position, Quaternion.identity);
            _currentLevel.InitLevel(this);
        }
        else
        {
           
            foreach (Enemy enemy in _currentLevel.EnemyList)
                Destroy(enemy.gameObject);
            _currentLevel.EnemyList.Clear();

            _currentLevel.InitLevel();
        }

        _currentGameState = GameState.PrepareGame;
        _currentHazzard = 0;
        //OnGameStart?.Invoke();
    }

    public void AddNewEnabledSpell(GameObject spell)
    {
        if(!_enabledSpells.Contains(spell))
            _enabledSpells.Add(spell);
    }

    private void OnEnable()
    {
        UIManager.OnUnPauseButtonClick += UIManager_OnUnPauseButtonClick;
        UIManager.OnToLevelsButtonClick += UIManager_OnToLevelsButtonClick;
        UIManager.OnToMainMenyButtonClick += UIManager_OnToMainMenyButtonClick;
    }

    private void Update()
    {
        switch (_currentGameState)
        {
            case GameState.PrepareGame:

                PauseGame();
                _currentHazzard = _currentLevel.EnemyList.Count;

                if (_currentHazzard >= _startingGameHazzard)
                {
                    SpawnPlayer(_platform);
                    _currentGameState = GameState.Playing;
                }

                break;
            case GameState.Playing:

                PauseGame();

                _currentHazzard = _currentLevel.EnemyList.Count;

                if (_currentHazzard == 0)
                {
                    _currentGameState = GameState.Win;
                }
                else if (_currentHazzard == _deathHazzardLevel)
                {
                    _currentGameState = GameState.Defeat;
                }
                break;
            case GameState.Win:
                Winlevel();
                _currentGameState = GameState.EndGame;
                break;
            case GameState.Defeat:
                LooseLevel();
                _currentGameState = GameState.EndGame; ;
                break;
            case GameState.EndGame:
                return;
        }

        if (isPause)
        {
            Time.timeScale = 0;
            OnPause?.Invoke();
        }
        else
        {
            OnUnPause?.Invoke();
            Time.timeScale = 1;
        }
    }

    private void UIManager_OnUnPauseButtonClick()
    {
        isPause = false;
    }

    private void UIManager_OnToLevelsButtonClick()
    {
        isPause = false;
        UnactiveCurrentLevel();
    }

    private void UIManager_OnToMainMenyButtonClick()
    {
        isPause = false;
        UnactiveCurrentLevel();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPause = !isPause;
    }

    private void SpawnPlayer(PlayerMove player)
    {
        PlayerMove createdPlayer = Instantiate(player);
        createdPlayer.transform.position = _currentLevel.SpawnPoint.position;
        createdPlayer.SetLimites(_currentLevel.LeftLim, _currentLevel.RightLim);
    }

    private void LooseLevel()
    {
        OnLoose?.Invoke();
        UnactiveCurrentLevel();
    }

    private void Winlevel()
    {
        OnWin?.Invoke();
        UnactiveCurrentLevel();
    }

    private void UnactiveCurrentLevel()
    {
        _currentLevel.gameObject.SetActive(false);
        OnDestroyPlayer?.Invoke();
    }
}

enum GameState
{
    NotGame,
    PrepareGame,
    Playing,
    Win,
    Defeat,
    EndGame
}
