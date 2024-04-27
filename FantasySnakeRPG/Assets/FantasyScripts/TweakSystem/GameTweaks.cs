using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameTweaks", menuName = "ScriptableObjects/GameTweaks", order = 1)]
public class GameTweaks : ScriptableObject
{
    [FormerlySerializedAs("GridSize_X")]
    [Header("Grid board data")]
    [Range(8,16)]
    public int Board_Row_Size  = 16;
    [Range(8,16)]
    public int Board_Column_Size = 16;

    [Header("Weight For Spawning")]
    [Range(1, 10)]
    public int WeightForGroud = 5;
    [Range(1, 10)]
    public int WeightForHero = 4;
    [Range(1, 10)]
    public int WeightForMonster = 4;
    [Range(1, 10)]
    public int WeightForObstacle = 3;
    
    [Header("Obstacle data")]
    public int ObstacleMinSize_X = 1;
    public int ObstacleMaxSize_X = 2;
    public int ObstacleMinSize_Y = 1;
    public int ObstacleMaxSize_Y = 2;
    
    [Header("Hero spawn max 25%")]
    [Range(1, 25)]
    public float HeroSpawnPercent = 21;
    [Header("Monster spawn max 25%")]
    [Range(1, 25)]
    public float MonsterSpawnPercent = 21;
    [Header("Obstacle spawn max 15%")]
    [Range(1, 15)]
    public float ObstacleSpawnPercent = 10;
    
    [Header("Hero Stats")]
    public int MinHeroHealth = 10;
    public int MaxHeroHealth = 40;
    [Header("Monster Stats")]
    public int MinMonsterHealth = 10;
    public int MaxMonsterHealth = 50;

    [Header("Hero Stat Growth")]
    public int MinHeroHealthPerMove = 1;
    public int MaxHeroHealthPerMove = 5;
    
    [Header("Monster Stat Growth")]
    public int MinMonsterHealthPerMove = 1;
    public int MaxMonsterHealthPerMove = 5;

    [Header("Debug spawn amount")]
    public int heroPossibleSpawnAmount;
    public int monsterPossibleSpawnAmount;
    public int obstaclePossibleSpawnAmount;

    private List<KeyValuePair<Globals.PoolType, int>> poolTypeWeightPair = new List<KeyValuePair<Globals.PoolType, int>>();

    public List<KeyValuePair<Globals.PoolType, int>>  GetSpawnWeightPairList()
    {
        if (poolTypeWeightPair.Count == 0)
        {
            poolTypeWeightPair = new List<KeyValuePair<Globals.PoolType, int>>();
            
            poolTypeWeightPair.Add(new KeyValuePair<Globals.PoolType, int>(Globals.PoolType.Ground,WeightForGroud));
            poolTypeWeightPair.Add(new KeyValuePair<Globals.PoolType, int>(Globals.PoolType.Hero,WeightForHero));
            poolTypeWeightPair.Add(new KeyValuePair<Globals.PoolType, int>(Globals.PoolType.Monster,WeightForMonster));
            poolTypeWeightPair.Add(new KeyValuePair<Globals.PoolType, int>(Globals.PoolType.Obstacle,WeightForObstacle));
            poolTypeWeightPair.Sort(delegate(KeyValuePair<Globals.PoolType, int>  firstPair, KeyValuePair<Globals.PoolType, int>  nextPair)
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                }
            );
        }

    
        return poolTypeWeightPair;
    }

    public void SetupSpawnPossibleAmount()
    {
        int boardSize = Board_Row_Size * Board_Column_Size;
        obstaclePossibleSpawnAmount = Mathf.CeilToInt(ObstacleSpawnPercent / 100 * boardSize);
        heroPossibleSpawnAmount = Mathf.CeilToInt(HeroSpawnPercent / 100 * boardSize);
        monsterPossibleSpawnAmount = Mathf.CeilToInt(MonsterSpawnPercent / 100 * boardSize);
    }
}
