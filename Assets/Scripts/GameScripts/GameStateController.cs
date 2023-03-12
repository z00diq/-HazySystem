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
    public static Action<float> OnGameStart;
    public static Action<float> OnEnemyCountChanged;

    public Level CurrentLevel => _currentLevel;
    public int CurrentLevelIndex;
    //public static Action OnGameStart;
    [SerializeField] private float _startingGameHazzard;  
    [SerializeField] private float _deathHazzardLevel;  
    [SerializeField] private List<GameObject> _enabledSpells;
    [SerializeField] private PlayerMove _platform;
    [SerializeField] private LevelManager _levelManager;
    private float _currentHazzard; 
    private GameState _currentGameState;

    private Level _currentLevel;
    private int _currentIndex;
    private bool isPause=false;

    public void InitLevel(int index)
    {
        if (_currentLevel!=null)
        {
            Destroy(_currentLevel.gameObject);
        }

        Level level = _levelManager.GetLevel(index);
        _currentLevel = Instantiate(level, transform.position, Quaternion.identity);
        _currentLevel.InitLevel(this);
      
        _currentGameState = GameState.PrepareGame;
        _currentHazzard = 0;
        _currentIndex = index;
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
                OnGameStart?.Invoke(_deathHazzardLevel);
                PauseGame();

                if (_currentLevel.EnemyList.Count >= _startingGameHazzard)
                {
                    SpawnPlayer(_platform);
                    _currentGameState = GameState.Playing;
                }

                break;
            case GameState.Playing:

                PauseGame();

                if (_currentHazzard != _currentLevel.EnemyList.Count)
                {
                    _currentHazzard = _currentLevel.EnemyList.Count;
                    OnEnemyCountChanged?.Invoke(_currentHazzard);
                }
                
                if (_currentHazzard == 0)
                {
                    _currentGameState = GameState.Win;
                }
                else if (_currentHazzard >= _deathHazzardLevel)
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
