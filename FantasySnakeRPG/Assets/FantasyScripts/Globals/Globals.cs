using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public const string HighScoreKey = "HighScore";
    public const string MoveCountKey = "Move";
    public const string MonsterDefeatedKey = "Mon_Defeated";
    
    public enum PoolType
    {
        BG = 0,
        Hero = 1,
        Monster = 2,
        Obstacle = 3,
        Ground = 4
    }
    
    public static Color IsPartyLeaderColor = new Color(1f, 0.42f, 0.06f);
    public static Color IsInPartyColor = new Color(1f, 0.63f, 0.82f);
    public static Color DefaultHeroColor = new Color(0.78f, 1f, 0.66f);
}
