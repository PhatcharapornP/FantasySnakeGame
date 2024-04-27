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
        panel.gameObject.SetActive(true);
    }

    protected override void OnTriggerHidePopup()
    {
        panel.gameObject.SetActive(false);
    }
}
