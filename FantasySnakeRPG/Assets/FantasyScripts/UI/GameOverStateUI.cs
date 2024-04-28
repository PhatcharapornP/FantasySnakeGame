using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverStateUI : BaseUserInterface
{
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button quitBtn;

    public override void Initialize()
    {
        replayBtn.onClick.AddListener(RestartGame);
        quitBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.StateManager.GoToMainMenuState();
        });
    }

    protected override void OnTriggerShowPopup()
    {
        panel.gameObject.SetActive(true);
    }

    protected override void OnTriggerHidePopup()
    {
        panel.gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        GameManager.Instance.StateManager.EndGameState();
        GameManager.Instance.StateManager.GoToGameState();
    }
}
