using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuUI : BaseUserInterface
{
    [SerializeField] private TextMeshProUGUI highScoreTxt;
    [SerializeField] private TextMeshProUGUI moveCountTxt;
    [SerializeField] private TextMeshProUGUI monsterCountTxt;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button resetHighScoreBtn;
    [SerializeField] private Button resetMoveBtn;
    [SerializeField] private Button resetMonsterBtn;
    [SerializeField] private Button quitToDesktopBtn;
    
    private const string HighScoreMsg = "Highest score:";
    private const string MoveCountMsg = "Highest move count:";
    private const string MonCountMsg = "Highest monster defeated:";
    
    public override void Initialize()
    {
        highScoreTxt.SetText($"{HighScoreMsg} {PlayerPrefs.GetInt(Globals.HighScoreKey)}");
        moveCountTxt.SetText($"{MoveCountMsg} {PlayerPrefs.GetInt(Globals.MoveCountKey)}");
        monsterCountTxt.SetText($"{MonCountMsg} {PlayerPrefs.GetInt(Globals.MonsterDefeatedKey)}");
        startBtn.onClick.AddListener(StartGameCallback);
        resetHighScoreBtn.onClick.AddListener(ResetHighScore);
        resetMoveBtn.onClick.AddListener(ResetMoveCount);
        resetMonsterBtn.onClick.AddListener(ResetMonsterCount);
        quitToDesktopBtn.onClick.AddListener(QuitToDesktop);
    }

    protected override void OnTriggerShowPopup()
    {
        //TODO: Can be change to animation state later
        panel.gameObject.SetActive(true);
    }

    protected override void OnTriggerHidePopup()
    {
        //TODO: Can be change to animation state later
        panel.gameObject.SetActive(false);
    }

    private void StartGameCallback()
    {
     GameManager.Instance.StartGame();   
    }

    private void ResetHighScore()
    {
        GameManager.Instance.PlayerScore.ResetScore();
        PlayerPrefs.SetInt(Globals.HighScoreKey,0);
        highScoreTxt.SetText($"{HighScoreMsg} {GameManager.Instance.PlayerScore.GetCurrentScore()}");
    }

    private void ResetMoveCount()
    {
        GameManager.Instance.MoveCounter.ResetMoveCounter();
        PlayerPrefs.SetInt(Globals.MoveCountKey,0);
        moveCountTxt.SetText($"{MoveCountMsg} {GameManager.Instance.MoveCounter.GetCurrentMoveAmount()}");
    }

    private void ResetMonsterCount()
    {
        GameManager.Instance.MonsterCounter.ResetMonsterAmount();
        PlayerPrefs.SetInt(Globals.MonsterDefeatedKey,0);
        monsterCountTxt.SetText($"{MonCountMsg} {GameManager.Instance.MonsterCounter.GetCurrentMonsterAmount()}");
    }

    private void QuitToDesktop()
    {
        GameManager.Instance.QuitToDesktop();
    }
}
