using UnityEngine;
using UnityEngine.UI;

public class Monster : BaseCharacterUnit
{
    [SerializeField] private Image heroStatusGraphic;
    protected override void OnRemoveUnitFromBoard()
    {
        gameObject.SetActive(false);
        //TODO: Need to spawn monster on random spot
    }

    protected override void OnSpawnOnBoard()
    {
        base.OnSpawnOnBoard();
        MaxHealth = Random.Range(GameManager.Instance.Tweaks.MinMonsterHealth,GameManager.Instance.Tweaks.MaxMonsterHealth + 1);
        Health = MaxHealth;
        Attack = Random.Range(GameManager.Instance.Tweaks.MinMonsterAttack,GameManager.Instance.Tweaks.MaxMonsterAttack + 1);
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
        heroStatusGraphic.fillAmount = tmpHealthPercent / 100;
    }
}
