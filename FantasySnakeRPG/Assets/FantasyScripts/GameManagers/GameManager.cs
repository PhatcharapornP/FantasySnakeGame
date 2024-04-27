using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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
    public Movecounter MoveCounter { get; private set; }
    public MonsterDefeatedCounter MonsterCounter { get; private set; }

    [SerializeField] private int currentScore = 0;

    private void Awake()
    {
        Instance = this;
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
    }
    
    public void StartGame()
    {
        gameStateManager.GoToGameState();
        MoveCounter.ResetMoveCounter();
        MonsterCounter.ResetMonsterAmount();
    }

    public void QuitToDesktop()
    {
        OverridePlayerStateAndSave();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    
    public void AddScore(int addOn)
    {
        currentScore += addOn;
    }
    
    public void SetHighScore(int score)
    {
        currentScore = score;
    }

    private void OverridePlayerStateAndSave()
    {
        int scoreTmp = PlayerPrefs.GetInt(Globals.HighScoreKey);
        if (scoreTmp < currentScore)
            PlayerPrefs.SetInt(Globals.HighScoreKey,currentScore);
        
        int moveTmp = PlayerPrefs.GetInt(Globals.MoveCountKey);
        if (moveTmp < MoveCounter.GetCurrentMoveAmount())
            PlayerPrefs.SetInt(Globals.MoveCountKey,MoveCounter.GetCurrentMoveAmount());
        
        int monTmp = PlayerPrefs.GetInt(Globals.MonsterDefeatedKey);
        if (monTmp < MonsterCounter.GetCurrentMonsterAmount())
            PlayerPrefs.SetInt(Globals.MonsterDefeatedKey,MonsterCounter.GetCurrentMonsterAmount());
    }
}
