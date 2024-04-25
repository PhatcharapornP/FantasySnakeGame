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
        quitBtn.onClick.AddListener(GameManager.Instance.QuitToDesktop);
    }

    protected override void OnTriggerShowPopup()
    {
        GameManager.Instance.UI.GameOver.OnShowPopup();
    }

    protected override void OnTriggerHidePopup()
    {
        GameManager.Instance.UI.GameOver.OnHidePopup();
    }

    private void RestartGame()
    {
        GameManager.Instance.StateManager.GoToGameState();
    }
}
