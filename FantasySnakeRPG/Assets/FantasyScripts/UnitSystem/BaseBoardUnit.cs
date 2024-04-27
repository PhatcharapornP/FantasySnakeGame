using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBoardUnit : MonoBehaviour,IBoardUnit
{
    public Image UnitGraphic;
    public Globals.PoolType UnitType { get; private set; }
    public Vector2Int BoardPosition { get; set; }
    public Vector3 GameobjPosition { get; set; }
    public Vector3 GameobjScale { get; set; }

    protected abstract void OnSelfUnitContact(IBoardUnit otherBoardUnit);

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

    protected virtual void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
    }
    
    public void RemoveUnitFromBoard()
    {
        OnRemoveUnitFromBoard();
    }

    public void OnUnitContact(IBoardUnit otherBoardUnit)
    {
        OnSelfUnitContact(otherBoardUnit);
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

}
