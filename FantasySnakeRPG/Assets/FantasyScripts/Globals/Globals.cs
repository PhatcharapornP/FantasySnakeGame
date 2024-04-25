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
        Obstacle = 3
    }
}
