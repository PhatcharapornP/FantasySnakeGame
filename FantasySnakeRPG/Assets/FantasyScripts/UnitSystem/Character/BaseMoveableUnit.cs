using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMoveableUnit : MonoBehaviour,IMoveableUnit
{
    public Vector2Int CurrentPos { get; set; }
    public Vector2Int PreviousPos { get; set; }
    public Vector2Int CurrentDirection { get; set; }
}
