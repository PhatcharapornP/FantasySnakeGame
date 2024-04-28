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
        
        MaxHealth = Random.Range(GameManager.Instance.Tweaks.MinHeroHealth,GameManager.Instance.Tweaks.MaxHeroHealth + 1);
        Health = MaxHealth;
        Attack = Random.Range(GameManager.Instance.Tweaks.MinHeroAttack,GameManager.Instance.Tweaks.MaxHeroAttack + 1);
    }
    
    public void SetHeroStatusColor(Color targetColor)
    {
        heroStatusGraphic.color = targetColor;
    }

    private float tmpHealthPercent = 0;

    protected override void OnHealthGotReduced()
    {
        base.OnHealthGotReduced();
        tmpHealthPercent = ((float)Health / (float)MaxHealth) * 100f;
        Debug.Log($"hero: {this} tmpHealthPercent: {tmpHealthPercent} ".InColor(new Color(1f, 0.24f, 0.76f)),this);
        heroStatusGraphic.fillAmount = tmpHealthPercent / 100f;
        Debug.Log($"------------------------Health: {Health} / MaxHealth: {MaxHealth} > | heroStatusGraphic.fillAmount: {heroStatusGraphic.fillAmount}-------------------".InColor(new Color(1f, 0.24f, 0.76f)),this);
    }
}
