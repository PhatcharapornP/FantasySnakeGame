using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateUI : BaseUserInterface
{
    public override void Initialize()
    {
    }

    protected override void OnTriggerShowPopup()
    {
        GameManager.Instance.UI.Battle.OnShowPopup();
    }

    protected override void OnTriggerHidePopup()
    {
        GameManager.Instance.UI.Battle.OnHidePopup();
    }
}
