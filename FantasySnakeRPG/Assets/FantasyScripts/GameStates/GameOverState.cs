using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : BaseState
{
    public override void Initialize()
    {
        
    }

    protected override void OnStartState()
    {
        GameManager.Instance.UI.GameOver.OnShowPopup();
        GameManager.Instance.UpdatePlayerStatAndSave();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.GameOver.OnHidePopup();
        
    }
}
