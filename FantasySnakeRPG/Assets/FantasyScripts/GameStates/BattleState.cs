using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BattleState : BaseState
{
    [SerializeField] private BaseCharacterUnit currentHero;
    [SerializeField] private BaseCharacterUnit currentMonster;
    private Vector2Int currentMonsterPos;
    
    public override void Initialize()
    {
    }

    protected override void OnStartState()
    {
        GameManager.Instance.UI.Battle.OnShowPopup();
    }

    protected override void OnEndState()
    {
        GameManager.Instance.UI.Battle.OnHidePopup();
        if (GameManager.Instance.UI.HudUI.IsPaused)
            GameManager.Instance.UI.HudUI.UnpauseGame();
    }

    public void SetupBattleParticipant(BaseCharacterUnit hero,BaseCharacterUnit monster)
    {
        currentHero = hero;
        currentMonster = monster;
        GameManager.Instance.UI.Battle.SetupHeroHealthBar(currentHero.Health,currentHero.MaxHealth);
        GameManager.Instance.UI.Battle.SetupMonsterHealthBar(currentMonster.Health,currentMonster.MaxHealth);
        GameManager.Instance.StateManager.GoToBattleState();
        StartBattle();
    }

    private int attackRNG;
    private void StartBattle()
    {
        currentMonsterPos = currentMonster.BoardPosition;
        GameManager.Instance.UI.Battle.SetHeroHealthBar(currentHero.Health);
        GameManager.Instance.UI.Battle.SetMonsterHealthBar(currentMonster.Health);
        attackRNG= Random.Range(0, 2);
        StartCoroutine(AttackOpponent(currentHero, currentMonster, () =>
        {
            GameManager.Instance.UI.Battle.SetHeroHealthBar(currentHero.Health);
            GameManager.Instance.UI.Battle.SetMonsterHealthBar(currentMonster.Health);
        }));
    }

    private IEnumerator AttackOpponent(BaseCharacterUnit hero ,BaseCharacterUnit monster,UnityAction callback)
    {
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.UI.Battle.TriggerMonsterAnimation(Globals.ATKAnim);
        GameManager.Instance.UI.Battle.TriggerHeroAnimation(Globals.ATKAnim);
        currentHero.ReduceHealth(currentMonster.Attack);
        currentMonster.ReduceHealth(currentHero.Attack);
        GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero attacking monster {hero.Attack} dmg");
        GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster attacking hero {monster.Attack} dmg");
        callback?.Invoke();
        yield return new WaitForSeconds(1.5f);
        
        while (hero.IsAlive && monster.IsAlive)
        {
            if (attackRNG == 0)
            {
                GameManager.Instance.UI.Battle.TriggerMonsterAnimation(Globals.ATKAnim);
                hero.ReduceHealth(monster.Attack);
                yield return new WaitForNextFrameUnit();
                GameManager.Instance.UI.Battle.TriggerHeroAnimation(Globals.HurtAnim);
                GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero attacking monster {hero.Attack} dmg");
                attackRNG = 1;
                callback?.Invoke();    
            }
            else
            {
                GameManager.Instance.UI.Battle.TriggerHeroAnimation(Globals.ATKAnim);
                monster.ReduceHealth(hero.Attack);
                yield return new WaitForNextFrameUnit();
                GameManager.Instance.UI.Battle.TriggerMonsterAnimation(Globals.HurtAnim);
                GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster attacking hero {monster.Attack} dmg");
                attackRNG = 0;
                callback?.Invoke();
            }
            
            yield return new WaitForSeconds(1.5f);
        }
        
        
        if (!currentMonster.IsAlive)
        {
            GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster defeated");
            GameManager.Instance.UI.Battle.TriggerMonsterAnimation(Globals.DieAnim);
            GameManager.Instance.Board.CompletelyRemoveFromBoard(currentMonster);
            currentHero.MoveUnit(currentMonsterPos);
            Player.Instance.MovePlayerPartySnake(true);
            GameManager.Instance.Board.FillInRandomGroundWithNewMonster();
            GameManager.Instance.MonsterCounter.IncreaseMonsterCounter();
            GameManager.Instance.PlayerScore.AddScore(Mathf.CeilToInt(5*GameManager.Instance.Tweaks.monsterDefeatScoreMultiplier));
            
            GameManager.Instance.UI.Game.SetMonsterDefeatedText($"{Globals.MonsterDefeatMsg}: {GameManager.Instance.MonsterCounter.GetCurrentMonsterAmount()}");
            GameManager.Instance.UI.Game.SetScoreText($"{Globals.HighScoreMsg}: {GameManager.Instance.PlayerScore.GetCurrentScore()}");
        }
        else
        {
            GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster health remain: {monster.Health}");
            GameManager.Instance.UI.Battle.TriggerMonsterAnimation(Globals.VictoryAnim);
        }
        
        yield return new WaitForSeconds(.5f);

        if (!currentHero.IsAlive)
        {
            GameManager.Instance.Board.CompletelyRemoveFromBoard(currentHero);
            GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero defeated");
            GameManager.Instance.UI.Battle.TriggerHeroAnimation(Globals.DieAnim);
        }
        else
        {
            GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero health remain: {hero.Health}");
            GameManager.Instance.UI.Battle.TriggerHeroAnimation(Globals.VictoryAnim);
        }
        
        

        yield return new WaitForSeconds(1.5f);
        
        GameManager.Instance.StateManager.OnBattleEnds();

        yield return null;
    }
}
