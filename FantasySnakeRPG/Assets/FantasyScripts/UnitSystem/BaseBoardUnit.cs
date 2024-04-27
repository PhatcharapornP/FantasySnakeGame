using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBoardUnit : MonoBehaviour,IBoardUnit
{
    public Image UnitGraphic;
    public Globals.PoolType UnitType { get; private set; }
    public Vector2Int BoardPosition { get; set; }
    public Vector3 GameobjPosition { get; set; }
    public Vector3 GameobjScale { get; set; }

    protected abstract void OnSelfUnitContactWhileMoving(IBoardUnit otherBoardUnit);
    
    protected virtual void OnSpawnOnBoard()
    {
        gameObject.SetActive(true);
    }

    protected virtual void OnSpawnInPool(Globals.PoolType _type)
    {
        UnitType = _type;
        gameObject.SetActive(false);
    }

    protected virtual void OnPullFromPool()
    {
    }

    protected abstract void OnRemoveUnitFromBoard();
    
    public void RemoveUnitFromBoard()
    {
        OnRemoveUnitFromBoard();
    }

    public void OnUnitContact(IBoardUnit otherBoardUnit)
    {
        OnSelfUnitContactWhileMoving(otherBoardUnit);
    }

    public void OnUnitSpawnInPool(Globals.PoolType _type)
    {
        OnSpawnInPool(_type);
    }

    public void OnUnitPullFromPool()
    {
        OnPullFromPool();
    }

    public void OnUnitSpawnOnBoard()
    {
        OnSpawnOnBoard();
    }

    public void SetupBoardPosData(Vector2Int boardPos)
    {
        BoardPosition = boardPos;
    }


    public void SetupUnitTransformPos(Vector3 targetPos)
    {
        GameobjPosition = targetPos;
    }

    public void SetupUnitScale(Vector3 gameObjScale)
    {
        GameobjScale = gameObjScale;
    }

    public void SetupUnitTrasnformOnScreen()
    {
       //TODO: set transform pos
        transform.localPosition = GameobjPosition;
        transform.localScale = GameobjScale;

    }

}
