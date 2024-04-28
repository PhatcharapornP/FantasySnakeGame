public class Monster : BaseCharacterUnit
{
    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
        //TODO: Need to spawn monster on random spot
    }
}
