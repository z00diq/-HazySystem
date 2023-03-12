using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject /*здесь будет тип спеллов* возможно это коллекци€*/ _unlockingSpell;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _leftLim;
    [SerializeField] private Transform _rightLim;
    
    private GameStateController _gameStateController;

    public List<Enemy> EnemyList => _enemyManager.EnemyList;
    public Transform SpawnPoint => _spawnPoint;
    public Transform LeftLim => _leftLim;
    public Transform RightLim => _rightLim;


    public void InitLevel(GameStateController gameStateController)
    {
        _gameStateController = gameStateController;
        GameStateController.OnWin += GameStateController_OnWin;
    }

    private void GameStateController_OnWin()
    {
        _gameStateController.AddNewEnabledSpell(_unlockingSpell);
    }
}
