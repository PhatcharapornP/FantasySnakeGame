using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController
{
    public bool IsCollisionAllowUnitToPass(IBoardUnit target, IBoardUnit theOtherUnit)
    {
        switch (theOtherUnit.UnitType)
        {
            case Globals.PoolType.Obstacle:
            {
                Debug.Log($"------------>{target.BoardPosition} collide with obstacle at {theOtherUnit.BoardPosition}".InColor(Color.red));
                GameManager.Instance.Board.CompletelyRemoveFromBoard((BaseBoardUnit)target);
                return false;
            }
            case Globals.PoolType.Hero:
            {
                if (Player.Instance.IsInPlayerParty((Hero)theOtherUnit) == false)
                {
                    //TODO: Means that it'll allow partyleader to replace this unit pos
                    //TODO: And when partyleader moves. the party follows
                    Player.Instance.AddHeroToPlayerParty((Hero)theOtherUnit);
                    GameManager.Instance.Board.FillInRandomGroundWithNewHero();
                    return true;
                }
                else
                {
                    if (Player.Instance.IsPartyLeader((Hero)target))
                    {
                        GameManager.Instance.StateManager.GoToGameOverState();
                        return false;
                    }
                    
                    return true;
                    
                }
            }
            case Globals.PoolType.Monster:
            {
                //TODO: For debug
                GameManager.Instance.Board.CompletelyRemoveFromBoard((BaseBoardUnit)target);
                //GameManager.Instance.StateManager.GoToBattleState();
                return false;
            }
            default: return false;
        }
    }

    public bool IsCollideWithOtherUnit(IBoardUnit target,Vector2Int pos)
    {
        var tmpUnitOnSpot = GameManager.Instance.Board.GetBoardUnitFromPos(pos);
        if (tmpUnitOnSpot)
        {
            if (IsCollisionAllowUnitToPass(target, tmpUnitOnSpot))
                return false; //Tell unit that it doesn't collide with anything since it's passable
            else
                return true; //Tell unit that it collided with something
        }
        else
        {
            return false; //Nothing in the way    
        }
    }
}
