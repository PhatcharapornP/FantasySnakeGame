public class MainMenuState : BaseState
{
    
    public override void Initialize()
    {
       
    }

    protected override void OnStartState()
    {
        GameManager.Instance.UI.MainMenu.OnShowPopup();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.MainMenu.OnHidePopup();
    }
}
