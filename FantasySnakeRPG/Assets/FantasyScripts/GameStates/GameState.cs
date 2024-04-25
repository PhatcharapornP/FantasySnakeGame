using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : BaseState
{
    public int PartyCount { get; private set; }
    [SerializeField] private List<Hero> playerParty = new List<Hero>();
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
    }

    public void RemoveHeroFromParty(Hero target)
    {
        playerParty.Remove(target);
    }

    public void SwitchSecondaryHeroToLeader()
    {
        
    }

    public void RotateLastHeroToLeader()
    {
        
    }
}
