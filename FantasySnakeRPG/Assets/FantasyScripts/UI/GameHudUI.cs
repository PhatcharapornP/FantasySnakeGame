using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHudUI : BaseUserInterface
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button unPauseBtn;
    [SerializeField] private Button quitBtn;
    public bool IsPaused { get; private set; }
    
    public override void Initialize()
    {
        pauseBtn.onClick.AddListener((PauseGame));
        unPauseBtn.onClick.AddListener(UnpauseGame);
        quitBtn.onClick.AddListener(GoToMainMenu);
    }

    protected override void OnTriggerShowPopup()
    {
        pauseBtn.gameObject.SetActive(true);
        if (IsPaused)
            panel.gameObject.SetActive(false);
    }

    protected override void OnTriggerHidePopup()
    {
        pauseBtn.gameObject.SetActive(false);
        if (IsPaused)
            panel.gameObject.SetActive(false);
    }
    
    private void PauseGame()
    {
        //TODO: Animation later
        panel.gameObject.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void UnpauseGame()
    {
        //TODO: Animation later
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;
        IsPaused = false;
    }

    private void GoToMainMenu()
    {
        GameManager.Instance.UpdatePlayerStatAndSave();
        GameManager.Instance.StateManager.GoToMainMenuState();
    }
}
