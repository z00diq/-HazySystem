using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    [SerializeField] private GameStateController _gameStateController;

    public void StartEnabledLevel(int index)
    {
        if(_gameStateController.CurrentLevel!=null)
            Destroy(_gameStateController.CurrentLevel.gameObject);

        _gameStateController.InitLevel(index);
    }

    internal Level GetLevel(int index)
    {
        return _levels[index];
    }
}
