using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    [SerializeField] private GameStateController _gameStateController;

    [ContextMenu("NextLVL")]
    public void StartEnabledLevel(int index)
    {
        _gameStateController.Initlevel(_levels[index]);
    }
}
