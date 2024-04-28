using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterUnit : BaseBoardUnit,IMoveableUnit,ICharacter
{
    public Vector2Int CurrentPos { get; set; }
    public Vector2Int PreviousPos { get; set; }
    public Vector2Int CurrentDirection { get; set; }
    protected BaseBoardUnit contactedUnit;
    
    public int Health { get; set; }
    public int Attack { get; set; }
    public bool IsAlive { get; set; }

    protected override void OnSpawnOnBoard()
    {
        base.OnSpawnOnBoard();
        IsAlive = true;
    }

    public bool MoveUnit(Vector2Int targetPos)
    {
        return OnMoveUnit(targetPos);
    }

    protected override void OnSelfUnitContactWhileMoving(IBoardUnit otherBoardUnit)
    {
        GameManager.Instance.CollisionControl.IsCollisionAllowUnitToPass(this,otherBoardUnit);
    }

    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
        IsAlive = false;
        
    }

    protected virtual bool OnMoveUnit(Vector2Int targetPos)
    {
        if (!IsAlive) return false;
        //TODO: Check on contact
        if (GameManager.Instance.CollisionControl.IsCollideWithOtherUnit(this, targetPos) == false)
        {
            PreviousPos = BoardPosition;
            SetupBoardPosData(targetPos);
            SetupUnitTransformPos(GameManager.Instance.Board.GetGameObjPos(targetPos));
            SetupUnitTrasnformOnScreen();
            return true;
        }
        else
            return false;
        
        
        
        // contactedUnit = GameManager.Instance.Board.TryUpdateUnitPosOnBoard(PreviousPos,targetPos, this); 
        // if (contactedUnit != null)
        // {
        //     OnSelfUnitContactWhileMoving(contactedUnit);
        // }
    }
}
