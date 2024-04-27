using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool HasPartyLeader => partyLeader != null;
    
    public Hero partyLeader { get; private set; }
    [SerializeField] private Vector2Int previousDirection;
    [SerializeField] private Vector2Int targetPos;
    [SerializeField] private List<Hero> playerParty = new List<Hero>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetPartyLeaderOnBoardCreation(Hero leader)
    {
        SetPartyLeader(leader);
        AddHeroToPlayerParty(leader);
    }

    public void SetPartyLeader(Hero leader)
    {
        partyLeader = leader;
        AddHeroToPlayerParty(partyLeader);
        partyLeader.SetHeroStatusColor(Globals.IsPartyLeaderColor);
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

    public bool IsInPlayerParty(Hero hero)
    {
        return playerParty.Contains(hero);
    }

    

    public void MovePartyLeader(Vector2Int direction)
    {
        if (GameManager.Instance.StateManager.currentState is GameState == false)
            return;
        
        if (direction.x != 0 && direction.y != 0) return;
        if (direction.x == 0 && direction.y == 0) return;

        if (previousDirection.x > 0 && direction.x < 0) return;
        if (previousDirection.x < 0 && direction.x > 0) return;

        if (previousDirection.y > 0 && direction.y < 0) return;
        if (previousDirection.y < 0 && direction.y > 0) return;

        targetPos = partyLeader.BoardPosition;

        targetPos.x += direction.x;
        targetPos.y += direction.y;
        previousDirection = direction;
        partyLeader.MoveUnit(targetPos);
    }
}
