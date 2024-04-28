using UnityEngine;
using UnityEngine.UI;

public class Hero : BaseCharacterUnit
{
    [SerializeField] private Image heroStatusGraphic;
    public Hero nodeToFollow;
    public bool IsSwitchingPartyLeader = false;
    protected override void OnRemoveUnitFromBoard()
    {
        base.OnRemoveUnitFromBoard();
        Debug.Log($"{GetType()} remove at: {BoardPosition}".InColor(Color.red),gameObject);
        SetHeroStatusColor(Globals.DefaultHeroColor);
        if (Player.Instance.IsInPlayerParty(this))
            Player.Instance.RemoveHeroFromParty(this);
        else
        {
            //TODO: Need to spawn hero on random spot
        }
        nodeToFollow = null;
        IsSwitchingPartyLeader = false;
    }

    protected override void OnSpawnOnBoard()
    {
        base.OnSpawnOnBoard();
        SetHeroStatusColor(Globals.DefaultHeroColor);
    }
    
    public void SetHeroStatusColor(Color targetColor)
    {
        heroStatusGraphic.color = targetColor;
    }

    protected override bool OnMoveUnit(Vector2Int targetPos)
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
