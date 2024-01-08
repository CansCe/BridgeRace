using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager 
{
    private static StateManager _instance;
    public static StateManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StateManager();
            }
            return _instance;
        }
    }

    public IState CurrentGameState { get; private set; }

    public delegate void GameStateChangeHandler(IState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;
    private StateManager()
    {

    }

    public void SetState(IState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;
        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }
}
