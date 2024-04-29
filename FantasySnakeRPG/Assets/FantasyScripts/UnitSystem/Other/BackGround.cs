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
