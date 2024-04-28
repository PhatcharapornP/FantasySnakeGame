public class Obstacle : BaseBoardUnit
{
    protected override void OnSelfUnitContactWhileMoving(IBoardUnit otherBoardUnit)
    {
    }

    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
    }
}
