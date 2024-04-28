using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BattleState : BaseState
{
    [SerializeField] private BaseCharacterUnit currentHero;
    [SerializeField] private BaseCharacterUnit currentMonster;
    
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

    private void StartBattle()
    {
        StartCoroutine(AttackOpponent(currentHero, currentMonster, () =>
        {
            GameManager.Instance.UI.Battle.SetHeroHealthBar(currentHero.Health);
            GameManager.Instance.UI.Battle.SetMonsterHealthBar(currentMonster.Health);
        }));
    }

    private IEnumerator AttackOpponent(BaseCharacterUnit hero ,BaseCharacterUnit monster,UnityAction callback)
    {
        yield return new WaitForSeconds(.5f);
        while (hero.IsAlive && monster.IsAlive)
        {
            monster.ReduceHealth(hero.Attack);
            GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero attacking monster {hero.Attack} dmg");
            callback?.Invoke();
            
            yield return new WaitForSeconds(1.5f);
            
            if (!hero.IsAlive || monster.IsAlive)
            {
                hero.ReduceHealth(monster.Attack);
                GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster attacking hero {monster.Attack} dmg");
                
                callback?.Invoke();
                yield return new WaitForSeconds(1.5f);
            }
            else
                break;
        }

        if (!currentHero.IsAlive)
        {
            GameManager.Instance.Board.CompletelyRemoveFromBoard(currentHero);
            GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero defeated");
        }
        else
            GameManager.Instance.UI.Battle.AddHeroActionMsg($"Hero health remain: {hero.Health}");
        
        if (!currentMonster.IsAlive)
        {
            GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster defeated");
            GameManager.Instance.Board.CompletelyRemoveFromBoard(currentMonster);
            var tmpPos = currentHero.BoardPosition;
            currentHero.MoveUnit(currentMonster.BoardPosition);
            Player.Instance.MovePlayerPartySnake(tmpPos,true);
            GameManager.Instance.Board.FillInRandomGroundWithNewMonster();
            GameManager.Instance.MonsterCounter.IncreaseMonsterCounter();
            GameManager.Instance.PlayerScore.AddScore(Mathf.CeilToInt(5*GameManager.Instance.Tweaks.monsterDefeatScoreMultiplier));
            
            GameManager.Instance.UI.Game.SetMonsterDefeatedText($"{Globals.MonsterDefeatMsg}: {GameManager.Instance.MonsterCounter.GetCurrentMonsterAmount()}");
            GameManager.Instance.UI.Game.SetScoreText($"{Globals.HighScoreMsg}: {GameManager.Instance.PlayerScore.GetCurrentScore()}");
        }
        else
            GameManager.Instance.UI.Battle.AddMonsterActionMsg($"Monster health remain: {monster.Health}");
        
        yield return new WaitForSeconds(1.5f);
        
        GameManager.Instance.StateManager.OnBattleEnds();

        yield return null;
    }
}
