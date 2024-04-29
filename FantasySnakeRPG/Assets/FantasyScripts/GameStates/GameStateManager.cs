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
    public BattleState Battle => battleState;
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
        if (currentState is BattleState)
        {
            currentState.EndState();
            gameState.EndState();
        }
        else
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
        currentState = battleState;
        battleState.StartState();
    }

    public void OnBattleEnds()
    {
        if (currentState is BattleState == false) return;
        if (Player.Instance.partyLeader == null)
        {
            
        }
        battleState.EndState();
        currentState = gameState;
    }

    public void GoToGameOverState(UnityAction callback = null)
    {
        if (currentState is GameOverState) return;
        EndCurrentState();
        currentState = gameOverState;
        currentState.StartState();
    }

    private void EndCurrentState()
    {
        if (currentState != null)
            currentState.EndState();
    }

    public void EndGameState()
    {
        if (gameState != null)
            gameState.EndState();
    }

    public void ResetGame()
    {
        if (GameManager.Instance.UI.HudUI.IsPaused)
            GameManager.Instance.UI.HudUI.UnpauseGame();
        Player.Instance.ResetAllParty();
    }
}
