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
    public IState currentState { get; private set; }
    [SerializeField] private Behaviour debugCurrentState;
    public void Initialize()
    {
        mainMenuState.Initialize();
        gameState.Initialize();
        battleState.Initialize();
        gameOverState.Initialize();
    }

    public void GoToMainMenuState(UnityAction callback = null)
    {
        if (currentState is MainMenuState) return;
        EndCurrentState();
        currentState = mainMenuState;
        currentState.StartState();
    }

    public void GoToGameState(UnityAction callback = null)
    {
        if (currentState is GameState) return;
        if (GameManager.Instance.UI.HudUI.IsPaused) return;
        EndCurrentState();
        currentState = gameState;
        currentState.StartState();
    }

    public void GoToBattleState(UnityAction callback = null)
    {
        if (GameManager.Instance.UI.HudUI.IsPaused) return;
        if (currentState is GameState == false) return;
        if (currentState is BattleState) return;
        currentState = battleState;
        battleState.StartState();
    }

    public void GoToGameOverState(UnityAction callback = null)
    {
        if (currentState is GameOverState) return;
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            GoToBattleState();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            GoToGameState();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            if (Player.Instance && Player.Instance.HasPartyLeader)
                Player.Instance.RemoveHeroFromParty(Player.Instance.partyLeader);
        }

        debugCurrentState = (Behaviour)currentState;
    }
}
