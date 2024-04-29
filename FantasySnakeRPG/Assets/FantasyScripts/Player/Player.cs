using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool HasPartyLeader => partyLeader != null;
    
    public Hero partyLeader { get; private set; }
    [SerializeField] private Vector2Int previousDirection;
    [SerializeField] private Vector2Int targetPos;
    [SerializeField] private List<Hero> playerParty = new List<Hero>();
    [FormerlySerializedAs("partyPreviousPoses")] [FormerlySerializedAs("partPreviousPoses")] [SerializeField] private List<Vector2Int> currentPartyPoses = new List<Vector2Int>();
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
        currentPartyPoses.Clear();
        partyLeader = null;
    }

    private void SetPartyLeader(Hero leader)
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
                   var currentDest = partyLeader.CurrentDestination;
                    var tmpPartyLeader = playerParty[1];
                    SetPartyLeader(tmpPartyLeader);
                    UpdateCurrentPartyPoses();
                    playerParty.Remove(target);

                    var unit = GameManager.Instance.Board.GetBoardUnitFromPos(currentDest);
                    if (unit == null)
                    {
                        partyLeader.MoveUnitToTargetPos(currentDest);
                        //Party follow new previous pos
                        OverwritePartyPreviousPoses();
                        MovePlayerPartySnake(true);
                    }
                    else
                    {
                        //Normal follows
                        partyLeader.MoveUnitToTargetPos(oldPartyLeaderPos);
                        MovePlayerPartySnake(true);
                    }
                }
                else
                    playerParty.Remove(target);
            }
            else
                playerParty.Remove(target);
        }
        
        currentPartyPoses.Clear();
        
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
        RefreshNodeToFollows();
    }

    public void RotateLastHeroToPartyLeader()
    {
        if (playerParty.Count <= 1) return;
        
        var oldPartyLeader = partyLeader;
        var oldPartyLeaderPos = partyLeader.BoardPosition;
        var newPartyLeader = playerParty[playerParty.Count - 1];
        var newPartyLeaderPos = playerParty[playerParty.Count-1].BoardPosition;
        
        SetPartyLeader(newPartyLeader);
        partyLeader.MoveUnitToTargetPos(oldPartyLeaderPos);
        
        oldPartyLeader.SetHeroStatusColor(Globals.IsInPartyColor);
        oldPartyLeader.MoveUnitToTargetPos(newPartyLeaderPos);
        
        playerParty[0] = partyLeader;
        playerParty[playerParty.Count-1] = oldPartyLeader;
        RefreshNodeToFollows();
    }

    private void RefreshNodeToFollows()
    {
        for (int i = 1; i < playerParty.Count; i++)
        {
            playerParty[i].nodeToFollow = playerParty[i - 1];
        }
    }

    private void UpdateCurrentPartyPoses()
    {
        for (int i = 0; i < playerParty.Count; i++)
        {
            currentPartyPoses.Add(playerParty[i].BoardPosition);
        }
    }

    private void OverwritePartyPreviousPoses()
    {
        for (int i = 0; i < playerParty.Count; i++)
        {
            playerParty[i].PreviousPos = currentPartyPoses[i];
        }
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

        if (previousDirection.x == 1 && direction.x == -1) return; //Reject moving opposite side X
        if (previousDirection.x == -1 && direction.x == 1) return;

        if (previousDirection.y == 1 && direction.y == -1) return; //Reject moving opposide side Y
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
        
        partyLeader.CurrentDestination = targetPos;
        previousDirection = direction;
        GameManager.Instance.MoveCounter.IncreaseMoveCounter();
        GameManager.Instance.UI.Game.SetMoveText($"{Globals.MoveMsg}: {GameManager.Instance.MoveCounter.GetCurrentMoveAmount()}");
        
        GameManager.Instance.Board.IncreaseHeroStatPerMove();
        GameManager.Instance.Board.IncreaseMonsterStatPerMove();

        if (partyLeader.MoveUnit(targetPos))
            MovePlayerPartySnake();
    }

    public void MovePlayerPartySnake(bool byPassColliding = false)
    {
        for (int i = 1; i < playerParty.Count; i++)
        {
            followNodeDestination = playerParty[i - 1].PreviousPos;
            
            if (!byPassColliding)
                playerParty[i].MoveUnit(followNodeDestination);
            else
                playerParty[i].MoveUnitToTargetPos(followNodeDestination);
        }
    }
}
