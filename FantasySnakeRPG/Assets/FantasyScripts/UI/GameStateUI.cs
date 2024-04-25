using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : BaseUserInterface
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI monsterDefeatedTxt;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button unPauseBtn;
    [SerializeField] private Button quitBtn;
    
    public override void Initialize()
    {
        panel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        pauseBtn.onClick.AddListener((PauseGame));
        unPauseBtn.onClick.AddListener(UnpauseGame);
        quitBtn.onClick.AddListener(GoToMainMenu);
    }

    protected override void OnTriggerShowPopup()
    {
        //TODO: Animation later
        panel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
    }

    protected override void OnTriggerHidePopup()
    {
        panel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }

    private void PauseGame()
    {
        //TODO: Animation later
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        //TODO: Animation later
        pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void GoToMainMenu()
    {
        GameManager.Instance.StateManager.GoToMainMenuState();
    }
}
