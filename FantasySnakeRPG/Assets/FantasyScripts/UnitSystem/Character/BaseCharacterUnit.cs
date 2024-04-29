using UnityEngine;

public class BaseCharacterUnit : BaseBoardUnit,IMoveableUnit,ICharacter
{
    public Vector2Int CurrentPos { get; set; }
    public Vector2Int PreviousPos { get; set; }
    public Vector2Int CurrentDestination { get; set; }
    protected BaseBoardUnit contactedUnit;
    
    public int Health { get; set; }
    public int MaxHealth { get; set; }
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

    public void ReduceHealth(int amount)
    {
        Health -= amount;
        OnHealthGotReduced();
        IsAlive = Health > 0;
        if (Health < 0)
            Health = 0;
        
    }

    public void IncreaseHealthAmount(int amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, Health, MaxHealth);
        OnHealthIncreased();
    }

    public void SetHealth(int amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, Health, MaxHealth);
        OnHealthIncreased();
    }

    protected virtual void OnHealthGotReduced()
    {
    }

    protected virtual void OnHealthIncreased()
    {
        
    }

    protected virtual bool OnMoveUnit(Vector2Int targetPos)
    {
        if (!IsAlive) return false;
        if (GameManager.Instance.CollisionControl.IsCollideWithOtherUnit(this, targetPos) == false)
        {
            MoveUnitToTargetPos(targetPos);
            return true;
        }
        return false;
    }
    
    public void MoveUnitToTargetPos(Vector2Int targetPos)
    {
        PreviousPos = BoardPosition;
        GameManager.Instance.Board.UpdateUnitPosOnBoard(targetPos,PreviousPos,this);
        SetupBoardPosData(targetPos);
        SetupUnitTransformPos(GameManager.Instance.Board.GetGameObjPos(targetPos));
        SetupUnitTrasnformOnScreen();
        CurrentPos = BoardPosition;
    }
}
