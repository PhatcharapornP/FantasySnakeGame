using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameTweaks tweaks;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private UIController uiController;
    [SerializeField] private ObjectPoolManager pool;
    [SerializeField] private BoardManager board;
    public GameStateManager StateManager => gameStateManager;
    public UIController UI => uiController;
    public ObjectPoolManager Pool => pool;
    public BoardManager Board => board;
    public GameTweaks Tweaks => tweaks;
    public CollisionController CollisionControl { get; private set; }
    public Movecounter MoveCounter { get; private set; }
    public MonsterDefeatedCounter MonsterCounter { get; private set; }
    public PlayerScore PlayerScore { get; private set; }
    

    private void Awake()
    {
        Instance = this;
        CollisionControl = new CollisionController();
    }

    private void Start()
    {
        Initialize();
        gameStateManager.Initialize();
        uiController.Initialize();
        pool.Initialize(() =>
        {
            gameStateManager.GoToMainMenuState();
        });
        tweaks.SetupSpawnPossibleAmount();
    }

    private void Initialize()
    {
        if (!PlayerPrefs.HasKey(Globals.HighScoreKey))
            PlayerPrefs.SetInt(Globals.HighScoreKey,0);
        if (MoveCounter == null)
            MoveCounter = new Movecounter();
        MoveCounter.ResetMoveCounter();

        if (MonsterCounter == null)
            MonsterCounter = new MonsterDefeatedCounter();
        MonsterCounter.ResetMonsterAmount();

        if (PlayerScore == null)
            PlayerScore = new PlayerScore();
        PlayerScore.ResetScore();
    }
    
    public void StartGame()
    {
        gameStateManager.GoToGameState();
        MoveCounter.ResetMoveCounter();
        MonsterCounter.ResetMonsterAmount();
    }

    public void QuitToDesktop()
    {
        UpdatePlayerStatAndSave();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void UpdatePlayerStatAndSave()
    {
        int scoreTmp = PlayerPrefs.GetInt(Globals.HighScoreKey);
        if (scoreTmp < PlayerScore.GetCurrentScore())
            PlayerPrefs.SetInt(Globals.HighScoreKey, PlayerScore.GetCurrentScore());
        
        int moveTmp = PlayerPrefs.GetInt(Globals.MoveCountKey);
        if (moveTmp < MoveCounter.GetCurrentMoveAmount())
            PlayerPrefs.SetInt(Globals.MoveCountKey,MoveCounter.GetCurrentMoveAmount());
        
        int monTmp = PlayerPrefs.GetInt(Globals.MonsterDefeatedKey);
        if (monTmp < MonsterCounter.GetCurrentMonsterAmount())
            PlayerPrefs.SetInt(Globals.MonsterDefeatedKey,MonsterCounter.GetCurrentMonsterAmount());
    }
}
