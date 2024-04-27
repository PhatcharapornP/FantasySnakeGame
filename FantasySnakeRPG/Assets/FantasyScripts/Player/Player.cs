using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool HasPartyLeader => partyLeader != null;
    public Hero partyLeader { get; private set; }
    [SerializeField] private List<Hero> playerParty = new List<Hero>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnBoardCreation(Hero leader)
    {
        SetPartyLeader(leader);
        AddHeroToPlayerParty(leader);
        //TODO: Be given a starter hero
        //TODO: Spawn and plot at the base of the board    
    }

    public void SetPartyLeader(Hero leader)
    {
        partyLeader = leader;
        AddHeroToPlayerParty(partyLeader);
        partyLeader.SetHeroStatusColor(Globals.IsPartyLeaderColor);
    }

    public void RemovePartyLeader()
    {
        partyLeader = null;
    }
    
    public void AddHeroToPlayerParty(Hero target)
    {
        if (target != null && playerParty.Contains(target) == false)
        {
            playerParty.Add(target);
            target.SetHeroStatusColor(Globals.IsInPartyColor);
        }
            
    }

    public void RemoveHeroFromParty(Hero target)
    {
        if (target != null)
            playerParty.Remove(target);
        if (playerParty.Count <= 0)
            GameManager.Instance.StateManager.GoToGameOverState();
    }
    
    public void SwitchSecondaryHeroToPartyLeader()
    {
        
    }

    public void RotateLastHeroToPartyLeader()
    {
        
    }
}
