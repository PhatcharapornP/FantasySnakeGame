using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterUnit : BaseBoardUnit,IMoveableUnit,ICharacter
{
    public Vector2Int CurrentPos { get; set; }
    public Vector2Int PreviousPos { get; set; }
    public Vector2Int CurrentDirection { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }

    protected override void OnSelfUnitContact(IBoardUnit otherBoardUnit)
    {
        
    }


    
}
