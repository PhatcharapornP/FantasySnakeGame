using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    [Header("State container")] 
    [SerializeField] private MainMenuState mainMenuState;
    [SerializeField] private GameState gameState;
    [SerializeField] private BattleState battleState;
    [SerializeField] private GameOverState gameOverState;
    private IState currentState;
    public void Initialize()
    {
        mainMenuState.Initialize();
        gameState.Initialize();
        battleState.Initialize();
        gameOverState.Initialize();
    }

    public void GoToMainMenuState(UnityAction callback = null)
    {
        EndCurrentState();
        currentState = mainMenuState;
        currentState.StartState();
    }

    public void GoToGameState(UnityAction callback = null)
    {
        EndCurrentState();
        currentState = gameState;
        currentState.StartState();
    }

    public void GoToBattleState(UnityAction callback = null)
    {
        if (currentState is GameState == false) return;
        currentState = battleState;
        battleState.StartState();
    }

    public void GoToGameOverState(UnityAction callback = null)
    {
        EndCurrentState();
        if (currentState is BattleState)
            battleState.EndState();
        currentState = gameOverState;
        currentState.StartState();
    }

    private void EndCurrentState()
    {
        if (currentState != null)
            currentState.EndState();
    }
}
