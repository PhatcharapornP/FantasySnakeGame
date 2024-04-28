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
        GameManager.Instance.StateManager.ResetGame();

    }

   
}
