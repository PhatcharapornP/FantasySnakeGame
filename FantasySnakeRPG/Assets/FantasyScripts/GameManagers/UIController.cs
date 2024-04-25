using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI container")] 
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private GameStateUI gameUI;
    [SerializeField] private BattleStateUI battleUI;
    [SerializeField] private GameOverStateUI gameOverUI;
    public MainMenuUI MainMenu => mainMenuUI;
    public GameStateUI Game => gameUI;
    public BattleStateUI Battle => battleUI;
    public GameOverStateUI GameOver => gameOverUI;

    public void Initialize()
    {
        mainMenuUI.Initialize();
        gameUI.Initialize();
        battleUI.Initialize();
        gameOverUI.Initialize();
    }
    
    
}
