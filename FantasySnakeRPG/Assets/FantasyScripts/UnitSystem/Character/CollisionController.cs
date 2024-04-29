using UnityEngine;

public class CollisionController
{
    public bool IsCollisionAllowUnitToPass(IBoardUnit target, IBoardUnit theOtherUnit)
    {
        switch (theOtherUnit.UnitType)
        {
            case Globals.PoolType.Obstacle:
            {
                GameManager.Instance.Board.CompletelyRemoveFromBoard((BaseBoardUnit)target);
                return false;
            }
            case Globals.PoolType.Hero:
            {
                if (Player.Instance.IsInPlayerParty((Hero)theOtherUnit) == false)
                {
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
                GameManager.Instance.StateManager.Battle.SetupBattleParticipant((Hero)target,(Monster)theOtherUnit);
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
            return false; //Nothing in the way    
    }
}
