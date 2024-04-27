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
        GameManager.Instance.UI.Game.OnShowPopup();
        GameManager.Instance.Board.GenerateBoard();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.Game.OnHidePopup();
        if (GameManager.Instance.UI.HudUI.IsPaused)
            GameManager.Instance.UI.HudUI.UnpauseGame();
        GameManager.Instance.Board.Clearboard();
    }

   
}
