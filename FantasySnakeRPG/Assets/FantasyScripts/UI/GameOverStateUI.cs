using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverStateUI : BaseUserInterface
{
    [SerializeField] private TextMeshProUGUI monsterdefeatedTxt;
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
        monsterdefeatedTxt.SetText($"{Globals.MonsterDefeatMsg}: {GameManager.Instance.MonsterCounter.GetCurrentMonsterAmount()}");
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
