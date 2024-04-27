using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : BaseUserInterface
{
    [SerializeField] private TextMeshProUGUI monsterDefeatedTxt;
    
    public override void Initialize()
    {
        panel.gameObject.SetActive(false);
    }

    protected override void OnTriggerShowPopup()
    {
        //TODO: Animation later
        panel.gameObject.SetActive(true);
        GameManager.Instance.UI.HudUI.OnShowPopup();
    }

    protected override void OnTriggerHidePopup()
    {
        panel.gameObject.SetActive(false);
        GameManager.Instance.UI.HudUI.OnHidePopup();
    }

  
}
