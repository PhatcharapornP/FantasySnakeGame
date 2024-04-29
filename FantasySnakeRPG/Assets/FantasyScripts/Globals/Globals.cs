using UnityEngine;

public static class Globals
{
    public const string HighScoreKey = "HighScore";
    public const string MoveCountKey = "Move";
    public const string MonsterDefeatedKey = "Mon_Defeated";
    
    public const string HighScoreMsg = "Score";
    public const string MoveMsg = "Moves";
    public const string MonsterDefeatMsg = "Monster defeated";
    
    public const string ATKAnim = "Atk";
    public const string HurtAnim = "Hurt";
    public const string DieAnim = "Die";
    public const string VictoryAnim = "Victory";
    
    
    public enum PoolType
    {
        BG = 0,
        Hero = 1,
        Monster = 2,
        Obstacle = 3
    }
    
    public static Color IsPartyLeaderColor = new Color(1f, 0.38f, 0f);
    public static Color IsInPartyColor = new Color(0.2f, 1f, 0.1f);
    public static Color DefaultHeroColor = new Color(0.91f, 0.98f, 1f);
}
