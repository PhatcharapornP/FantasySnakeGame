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
        SetHeroStatusColor(Globals.DefaultHeroColor);
        if (Player.Instance.IsInPlayerParty(this))
            Player.Instance.RemoveHeroFromParty(this);
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
        UpdateHealthStatusUI();
    }

    protected override void OnHealthIncreased()
    {
        base.OnHealthIncreased();
        UpdateHealthStatusUI();
    }

    private void UpdateHealthStatusUI()
    {
        tmpHealthPercent = ((float)Health / (float)MaxHealth) * 100f;
        heroStatusGraphic.fillAmount = tmpHealthPercent / 100f;
    }
}
