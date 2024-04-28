using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : BaseState
{
    public override void Initialize()
    {
    }

    protected override void OnStartState()
    {
        GameManager.Instance.MonsterCounter.ResetMonsterAmount();
        GameManager.Instance.UI.Game.OnShowPopup();
        GameManager.Instance.MonsterCounter.ResetMonsterAmount();
        GameManager.Instance.PlayerScore.ResetScore();
        GameManager.Instance.MoveCounter.ResetMoveCounter();
        GameManager.Instance.UI.Game.SetMonsterDefeatedText($"{Globals.MonsterDefeatMsg}: {GameManager.Instance.MonsterCounter.GetCurrentMonsterAmount()}");
        GameManager.Instance.UI.Game.SetScoreText($"{Globals.HighScoreMsg}: {GameManager.Instance.PlayerScore.GetCurrentScore()}");
        GameManager.Instance.UI.Game.SetMoveText($"{Globals.MoveMsg}: {GameManager.Instance.PlayerScore.GetCurrentScore()}");
        GameManager.Instance.Board.GenerateBoard();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.Game.OnHidePopup();
        GameManager.Instance.StateManager.ResetGame();
        GameManager.Instance.UpdatePlayerStatAndSave();
    }
   
}
