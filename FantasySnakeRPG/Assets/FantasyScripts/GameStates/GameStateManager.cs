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
    public BattleState Battle => battleState;
    public GameState Game => gameState;
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
        // if (currentState is GameState == false) return;
        // if (currentState is BattleState) return;
        currentState = battleState;
        battleState.StartState();
    }

    public void OnBattleEnds()
    {
        //if (GameManager.Instance.UI.HudUI.IsPaused) return;
        Debug.Log($"OnBattleEnds {currentState}" .InColor(Color.red));
        if (currentState is BattleState == false) return;
        
        battleState.EndState();
        currentState = gameState;
        Debug.Log($"OnBattleEnds {currentState}".InColor(Color.green));
    }

    public void GoToGameOverState(UnityAction callback = null)
    {
        if (currentState is GameOverState) return;
        EndCurrentState();
        // if (currentState is BattleState)
        //     battleState.EndState();
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            GoToBattleState();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            OnBattleEnds();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            if (Player.Instance && Player.Instance.HasPartyLeader)
            {
                GameManager.Instance.Board.CompletelyRemoveFromBoard(Player.Instance.partyLeader);
            }
        }

        debugCurrentState = (Behaviour)currentState;
    }
}
