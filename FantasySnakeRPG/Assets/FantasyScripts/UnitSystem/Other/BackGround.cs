using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : BaseBoardUnit
{
    protected override void OnSelfUnitContactWhileMoving(IBoardUnit otherBoardUnit)
    {
    }

    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
    }
}
