using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject /*здесь будет тип спеллов* возможно это коллекци€*/ _unlockingSpell;

    private GameStateController _gameStateController;

    public void InitLevel(GameStateController gameStateController)
    {
        _gameStateController = gameStateController;
        _gameStateController.OnWin += GameStateController_OnWin;
    }

    private void GameStateController_OnWin()
    {
        _gameStateController.AddNewEnabledSpell(_unlockingSpell);
    }
}
