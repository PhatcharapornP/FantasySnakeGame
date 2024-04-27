using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : BaseState
{
    public override void Initialize()
    {
    }

    protected override void OnStartState()
    {
        GameManager.Instance.UI.Battle.OnShowPopup();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.Battle.OnHidePopup();
        if (GameManager.Instance.UI.HudUI.IsPaused)
            GameManager.Instance.UI.HudUI.UnpauseGame();
    }
}
