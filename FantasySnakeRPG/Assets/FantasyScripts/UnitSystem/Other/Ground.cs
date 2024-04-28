using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : BaseBoardUnit
{
    protected override void OnSelfUnitContactWhileMoving(IBoardUnit otherBoardUnit)
    {
    }

    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
        Debug.Log($"remove {GetType()} at: {BoardPosition}".InColor(Color.red),gameObject);
    }
}
