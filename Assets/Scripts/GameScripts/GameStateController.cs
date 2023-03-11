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

    public Level CurrentLevel => _currentLevel;

    //public static Action OnGameStart;
    [SerializeField] private float _startingGameHazzard;  /*количество заражения при котором игра начинается*/
    [SerializeField] private float _deathHazzardLevel;  /*количество заражения при котором игра закончится*/
    [SerializeField] private List<GameObject> _enabledSpells; /*список lоступных спеллов для получения игроком*/
    private float _currentHazzard; /*текущее количество заражения*/ 
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

                if (_currentHazzard >= _startingGameHazzard)
                {
                    _currentGameState = GameState.Playing;
                    //SpawnPlayer(playerPrefab); - вызов игрока на арену
                }

                break;
            case GameState.Playing:

                PauseGame();
                //Активизация игрового процесса

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
               

                if (Physics.Raycast(ray, out hit))
                    if (hit.collider.GetComponent<Enemy>())
                    {
                        Destroy(hit.collider.gameObject);
                    }

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

    private void Enemy_OnDeath(float hazzard)
    {
        _currentHazzard -= hazzard;
    }

    private void Enemy_OnBorn(float hazzard)
    {
        _currentHazzard += hazzard;
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

    private void SpawnPlayer(object playerPrefab)
    {
        throw new NotImplementedException();
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
    }
}

enum GameState
{
    PrepareGame,
    Playing,
    Win,
    Defeat,
    EndGame
}
