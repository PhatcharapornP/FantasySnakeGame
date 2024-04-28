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
    [SerializeField] private bool rejectingInput = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetAllParty()
    {
        previousDirection = Vector2Int.zero;
        targetPos = Vector2Int.zero;
        playerParty.Clear();
        partyLeader = null;
    }

    public void SetPartyLeader(Hero leader)
    {
        partyLeader = leader;
        partyLeader.SetHeroStatusColor(Globals.IsPartyLeaderColor);
    }
    
    public void AddHeroToPlayerParty(Hero target)
    {
        if ( playerParty.Contains(target) == false)
        {
            if (playerParty.Count <= 0)
            {
                SetPartyLeader(target);
                playerParty.Add(target);
            }
            else
            {
                playerParty.Add(target);
                target.SetHeroStatusColor(Globals.IsInPartyColor);
                target.nodeToFollow = playerParty[playerParty.Count -2]; //last node
            }
        }
    }

    public void RemoveHeroFromParty(Hero target)
    {
        if (target != null)
        {
            if (target == partyLeader)
            {
                //TODO: Select new party leader
                if (playerParty.Count > 1)
                {
                   var oldPartyLeaderPos = partyLeader.BoardPosition;
                    var tmpPartyLeader = playerParty[1];
                    var tmpNewPartyLeaderPos = playerParty[1].BoardPosition;
                    SetPartyLeader(tmpPartyLeader);
                    playerParty.Remove(target);
                     partyLeader.MoveUnitToTargetPos(oldPartyLeaderPos);
                     MovePlayerPartySnake(tmpNewPartyLeaderPos,true);
                     
                     Debug.Log($"RemoveHeroFromParty oldPartyLeaderPos: {oldPartyLeaderPos} | tmpNewPartyLeaderPos: {tmpNewPartyLeaderPos}".InColor(new Color(1f, 0.93f, 0.56f)),partyLeader);
                }
                else
                {
                    Debug.Log($"Removing {target.name} from pos: {target.BoardPosition} from party".InColor(new Color(0.74f, 0.91f, 1f)),target);
                    playerParty.Remove(target);
                }
            }
            else
            {
                Debug.Log($"Removing {target.name} from pos: {target.BoardPosition} from party".InColor(new Color(0.74f, 0.71f, 1f)),target);
                playerParty.Remove(target);
            }
        }
        
        if (playerParty.Count <= 0)
            GameManager.Instance.StateManager.GoToGameOverState();
    }
    
    public void SwitchSecondaryHeroToPartyLeader()
    {
        if (playerParty.Count <= 1) return;

        var previousPartyLeader = partyLeader;
        var newPartyLeaderPos = partyLeader.BoardPosition;
        var newPreviousPartyLeaderPos = playerParty[1].BoardPosition; 
        SetPartyLeader(playerParty[1]);
        
        //TODO: Update new party leader pos
        partyLeader.IsSwitchingPartyLeader = true;
        partyLeader.MoveUnitToTargetPos(newPartyLeaderPos);
        
        //TODO: Update previous party leader pos
        previousPartyLeader.IsSwitchingPartyLeader = true;
        previousPartyLeader.MoveUnitToTargetPos(newPreviousPartyLeaderPos);
        playerParty[0] = partyLeader;
        playerParty[1] = previousPartyLeader;
        previousPartyLeader.SetHeroStatusColor(Globals.IsInPartyColor);
    }

    public void RotateLastHeroToPartyLeader()
    {
        if (playerParty.Count <= 1) return;
        
        var oldPartyLeader = partyLeader;
        var oldPartyLeaderPos = partyLeader.BoardPosition;
        var newPartyLeader = playerParty[playerParty.Count - 1];
        var newPartyLeaderPos = playerParty[playerParty.Count-1].BoardPosition;
        
        //TODO: Set new Partyleader
        SetPartyLeader(newPartyLeader);
        
        //TODO: Set new partyLeader position with the old partyleader pos
        partyLeader.MoveUnitToTargetPos(oldPartyLeaderPos);
        
        //TODO: Set old Partyleader
        oldPartyLeader.SetHeroStatusColor(Globals.IsInPartyColor);
        
        //TODO: Set old partyLeader position
        oldPartyLeader.MoveUnitToTargetPos(newPartyLeaderPos);
        
        //TODO: Replace position in list
        playerParty[0] = partyLeader;
        playerParty[playerParty.Count-1] = oldPartyLeader;
        
         
        
        
        // //TODO: Update new party leader pos
        // partyLeader.IsSwitchingPartyLeader = true;
        //
        //
        // //TODO: Update previous party leader pos
        // oldPartyLeader.IsSwitchingPartyLeader = true;
        //
        // playerParty[0] = partyLeader;
        // playerParty[playerParty.Count-1] = oldPartyLeader;
    }

    public bool IsInPlayerParty(Hero hero)
    {
        return playerParty.Contains(hero);
    }

    public bool IsPartyLeader(Hero hero)
    {
        return partyLeader == hero;
    }

    private Vector2Int followNodeDestination;

    public void MovePartyLeader(Vector2Int direction)
    {
        rejectingInput = true;
        if (GameManager.Instance.StateManager.currentState is GameState == false)
            return;
        
        if (direction.x != 0 && direction.y != 0) return; //Reject when both direction is being pressed
        if (direction.x == 0 && direction.y == 0) return; //Reject if both direction doesn't have value

        if (previousDirection.x == 1 && direction.x == -1) return; //Reject moving opposite side
        if (previousDirection.x == -1 && direction.x == 1) return;

        if (previousDirection.y == 1 && direction.y == -1) return; //Reject moving opposide side
        if (previousDirection.y == -1 && direction.y == 1) return;

        targetPos = partyLeader.BoardPosition;
        Vector2Int tmpPartyLeaderPreviousBoardPos = targetPos;

        targetPos.x += direction.x;
        targetPos.y += direction.y;

        //Border checks
        if (targetPos.x > GameManager.Instance.Tweaks.Board_Column_Size-1 || targetPos.y > GameManager.Instance.Tweaks.Board_Row_Size-1)
            return;
        
        if (targetPos.x < 0 || targetPos.y < 0)
            return;
        
        rejectingInput = false;
        
        //TODO: Move the entire party
        partyLeader.CurrentDirection = direction;
        previousDirection = direction;
        GameManager.Instance.MoveCounter.IncreaseMoveCounter();
        GameManager.Instance.UI.Game.SetMoveText($"{Globals.MoveMsg}: {GameManager.Instance.MoveCounter.GetCurrentMoveAmount()}");
        if (partyLeader.MoveUnit(targetPos))
            MovePlayerPartySnake(tmpPartyLeaderPreviousBoardPos);
    }

    public void MovePlayerPartySnake(Vector2Int tmpPartyLeaderPreviousBoardPos,bool byPassColliding = false)
    {
        for (int i = 1; i < playerParty.Count; i++)
        {
            //TODO: Need to give direction to other nodes
            if (playerParty[i].nodeToFollow == partyLeader)
            {
                followNodeDestination = tmpPartyLeaderPreviousBoardPos;
            }
            else
                followNodeDestination = playerParty[i - 1].PreviousPos;
            
            if (!byPassColliding)
                playerParty[i].MoveUnit(followNodeDestination);
            else
                playerParty[i].MoveUnitToTargetPos(followNodeDestination);
        }
    }
}
