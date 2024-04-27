using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : BaseCharacterUnit
{
    [SerializeField] private Image heroStatusGraphic;
    protected override void OnRemoveUnitFromBoard()
    {
        base.OnRemoveUnitFromBoard();

        if (this == Player.Instance.partyLeader || Player.Instance.IsInPlayerParty(this))
            Player.Instance.RemoveHeroFromParty(this);
    }

    protected override void OnSpawnOnBoard()
    {
        base.OnSpawnOnBoard();
        SetHeroStatusColor(Globals.DefaultHeroColor);
    }
    
    public void SetHeroStatusColor(Color targetColor)
    {
        heroStatusGraphic.color = targetColor;
    }
}
