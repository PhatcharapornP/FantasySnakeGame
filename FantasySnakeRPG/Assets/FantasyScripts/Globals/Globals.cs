using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public const string HighScoreKey = "HighScore";
    public const string MoveCountKey = "Move";
    public const string MonsterDefeatedKey = "Mon_Defeated";
    
    public const string HighScoreMsg = "Score";
    public const string MoveMsg = "Moves";
    public const string MonsterDefeatMsg = "Monster defeated";
    
    public enum PoolType
    {
        BG = 0,
        Hero = 1,
        Monster = 2,
        Obstacle = 3,
        Ground = 4
    }
    
    public static Color IsPartyLeaderColor = new Color(1f, 0.44f, 0.09f);
    public static Color IsInPartyColor = new Color(0.85f, 1f, 0.47f);
    public static Color DefaultHeroColor = new Color(0.41f, 0.64f, 1f);
}
