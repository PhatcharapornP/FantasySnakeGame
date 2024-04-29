using UnityEngine;

public interface IMoveableUnit
{
    Vector2Int CurrentPos { get; set; }
    Vector2Int PreviousPos { get; set; }
    Vector2Int CurrentDestination { get; set; }

    bool MoveUnit(Vector2Int targetPos);
}
