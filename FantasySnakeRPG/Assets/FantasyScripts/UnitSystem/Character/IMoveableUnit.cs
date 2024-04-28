using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveableUnit
{
    Vector2Int CurrentPos { get; set; }
    Vector2Int PreviousPos { get; set; }
    Vector2Int CurrentDirection { get; set; }

    bool MoveUnit(Vector2Int targetPos);
}
