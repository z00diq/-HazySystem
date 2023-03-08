using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject /*здесь будет тип спеллов* возможно это коллекци€*/ _unlockingSpell;
    [SerializeField] private EnemyManager _enemyManager;

    private GameStateController _gameStateController;

    public void InitLevel(GameStateController gameStateController=null)
    {
        if (gameStateController != null)
        {
            _gameStateController = gameStateController;
            GameStateController.OnWin += GameStateController_OnWin;
        }
        else
        {
            gameObject.SetActive(true);
        }

        Invoke(nameof(InitEnemySpawner),3f);
    }

    private void InitEnemySpawner()
    {
        _enemyManager.Infestation();
    }

    private void GameStateController_OnWin()
    {
        _gameStateController.AddNewEnabledSpell(_unlockingSpell);
    }
}
