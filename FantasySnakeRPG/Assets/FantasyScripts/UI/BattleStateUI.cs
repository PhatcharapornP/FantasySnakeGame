using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleStateUI : BaseUserInterface
{
    [SerializeField] private Image heroImage;
    [SerializeField] private Image monsterImage;
    [SerializeField] private Slider heroHealthBar;
    [SerializeField] private Slider monsterHealthBar;
    [SerializeField] private Transform heroMsgContainer;
    [SerializeField] private Transform monsterMsgContainer;
    [SerializeField] private TextMeshProUGUI msgPrefab;
    
    public override void Initialize()
    {
        heroHealthBar.minValue = 0;
        monsterHealthBar.minValue = 0;
    }

    public void AddHeroActionMsg(string msg)
    {
        TextMeshProUGUI heroMsg = Instantiate(msgPrefab, heroMsgContainer);
        heroMsg.SetText(msg);
        Destroy(heroMsg.gameObject,1.5f);
    }

    public void AddMonsterActionMsg(string msg)
    {
        TextMeshProUGUI monsterMsg = Instantiate(msgPrefab, monsterMsgContainer);
        monsterMsg.SetText(msg);
        Destroy(monsterMsg.gameObject,1.5f);
    }

    protected override void OnTriggerShowPopup()
    {
        panel.gameObject.SetActive(true);
    }

    protected override void OnTriggerHidePopup()
    {
        panel.gameObject.SetActive(false);
    }

    public void SetupHeroHealthBar(int health,int max)
    {
        heroHealthBar.maxValue = max;
        SetHeroHealthBar(health);
    }

    public void SetupMonsterHealthBar(int health,int max)
    {
        monsterHealthBar.maxValue = max;
        SetMonsterHealthBar(health);
    }

    public void SetHeroHealthBar(int value)
    {
        heroHealthBar.value = value;
    }
    
    public void SetMonsterHealthBar(int value)
    {
        monsterHealthBar.value = value;
    }

}
