using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI container")] 
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private GameStateUI gameUI;
    [SerializeField] private BattleStateUI battleUI;
    [SerializeField] private GameOverStateUI gameOverUI;
    [SerializeField] private GameHudUI hudUI;
    public MainMenuUI MainMenu => mainMenuUI;
    public GameStateUI Game => gameUI;
    public BattleStateUI Battle => battleUI;
    public GameOverStateUI GameOver => gameOverUI;
    public GameHudUI HudUI => hudUI;

    public void Initialize()
    {
        mainMenuUI.Initialize();
        gameUI.Initialize();
        hudUI.Initialize();
        battleUI.Initialize();
        gameOverUI.Initialize();
    }
    
    
}
