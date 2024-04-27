using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterUnit : BaseBoardUnit,IMoveableUnit,ICharacter
{
    public Vector2Int CurrentPos { get; set; }
    public Vector2Int PreviousPos { get; set; }
    public Vector2Int CurrentDirection { get; set; }
    private BaseBoardUnit contactedUnit;
    
    public int Health { get; set; }
    public int Attack { get; set; }
    public bool IsAlive { get; set; }

    protected override void OnSpawnOnBoard()
    {
        base.OnSpawnOnBoard();
        IsAlive = true;
    }

    public void MoveUnit(Vector2Int targetPos)
    {
        OnMoveUnit(targetPos);
    }

    protected override void OnSelfUnitContactWhileMoving(IBoardUnit otherBoardUnit)
    {
        switch (otherBoardUnit.UnitType)
        {
            case Globals.PoolType.Obstacle:
            {
                RemoveUnitFromBoard();
            }
                break;
        }
        
        
    }

    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
        IsAlive = false;
    }

    protected virtual void OnMoveUnit(Vector2Int targetPos)
    {
        if (!IsAlive) return;
        PreviousPos = BoardPosition;
        SetupBoardPosData(targetPos);
        SetupUnitTransformPos(GameManager.Instance.Board.GetGameObjPos(targetPos));
        SetupUnitTrasnformOnScreen();
        contactedUnit = GameManager.Instance.Board.TryUpdateUnitPosOnBoard(PreviousPos,targetPos, this); 
        if (contactedUnit != null)
        {
            OnSelfUnitContactWhileMoving(contactedUnit);
        }
    }
}
