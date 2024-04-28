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
        // contactedUnit = GameManager.Instance.Board.TryUpdateUnitPosOnBoard(PreviousPos,targetPos, this);
        // if (!IsSwitchingPartyLeader)
        // {
        //     if (contactedUnit != null)
        //     {
        //         OnSelfUnitContactWhileMoving(contactedUnit);
        //     }    
        // }
        // else
        //     IsSwitchingPartyLeader = false;
    }

    public void MoveUnitToTargetPos(Vector2Int targetPos)
    {
        Debug.Log($"moving hero {gameObject.name} currentPos: {CurrentPos} | BoardPosition: {BoardPosition} to targetPos: {targetPos}".InColor(new Color(1f, 0.8f, 0.73f)),gameObject);
        //TODO: temp remove self from board
      //  GameManager.Instance.Board.TempRemoveFromBoard(BoardPosition);
        PreviousPos = BoardPosition;
        GameManager.Instance.Board.UpdateUnitPosOnBoard(targetPos,PreviousPos,this);
        SetupBoardPosData(targetPos);
        SetupUnitTransformPos(GameManager.Instance.Board.GetGameObjPos(targetPos));
        SetupUnitTrasnformOnScreen();
        CurrentPos = BoardPosition;
        // if (GameManager.Instance.Board.GetBoardUnitFromPos(PreviousPos) == null)
        // {
        //     GameManager.Instance.Board.FillGroundInBlankPos(PreviousPos);
        //     var tmp = GameManager.Instance.Board.GetBoardUnitFromPos(PreviousPos);
        //   //  Debug.Log($"{gameObject.name} > PreviousPos: {PreviousPos} | BoardPosition: {BoardPosition}".InColor(new Color(1f, 0.47f, 0.14f)),tmp);
        // }
        // else
        // {
        //   //  Debug.Log($"{gameObject.name} > PreviousPos: {PreviousPos} | BoardPosition: {BoardPosition}".InColor(new Color(0.6f, 1f, 0.22f)),gameObject);
        // }
    }
}
