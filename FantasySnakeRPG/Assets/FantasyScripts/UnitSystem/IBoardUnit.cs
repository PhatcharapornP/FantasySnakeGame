using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardUnit
{
    Globals.PoolType UnitType { get; }
    Vector2Int BoardPosition { get; set; }
    Vector3 GameobjPosition { get; set; }
    Vector3 GameobjScale { get; set; }
    void RemoveUnitFromBoard();
    void OnUnitContact(IBoardUnit otherBoardUnit);

    void OnUnitSpawnInPool(Globals.PoolType _type);
    void OnUnitPullFromPool();

    void OnUnitSpawnOnBoard();
}
