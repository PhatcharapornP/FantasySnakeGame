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
    public int MinHeroAttack = 20;
    public int MaxHeroAttack = 40;
    [Header("Monster Stats")]
    public int MinMonsterHealth = 10;
    public int MaxMonsterHealth = 50;
    public int MinMonsterAttack = 20;
    public int MaxMonsterAttack = 45;

    [Header("Hero Stat Growth")]
    public int MinHeroHealthPerMove = 1;
    public int MaxHeroHealthPerMove = 5;
    public int MinHeroAttackPerMove = 1;
    public int MaxHeroAttackPerMove = 5;
    
    [Header("Monster Stat Growth")]
    public int MinMonsterHealthPerMove = 1;
    public int MaxMonsterHealthPerMove = 5;
    public int MinMonsterAttackPerMove = 1;
    public int MaxMonsterAttackPerMove = 5;

    [Header("Defeat score")]
    [Range(1.1f,5)]
    public float monsterDefeatScoreMultiplier = 1.5f;
    
    [Header("How many spawn when removed")] 
    [Range(1, 5)]
    public int monsterSpawnOnRemoveAmount = 1;
    
    [Range(1, 5)]
    public int heroSpawnOnRemoveAmount = 1;

    [Header("Spawn amount")]
    public int MinHeroPossibleSpawnAmount;
    public int MinMonsterPossibleSpawnAmount;
    public int MaxHeroPossibleSpawnAmount;
    public int MaxMonsterPossibleSpawnAmount;
    public int MaxObstaclePossibleSpawnAmount;

    public void SetupSpawnPossibleAmount()
    {
        int fullBoardSize = Board_Row_Size * Board_Column_Size;
        float limitBoardSpawn = 80f / 100f * fullBoardSize;
        
        
        MinHeroPossibleSpawnAmount = Mathf.CeilToInt(HeroSpawnPercent / 100f * limitBoardSpawn);
        MinMonsterPossibleSpawnAmount = Mathf.CeilToInt(MonsterSpawnPercent / 100f * limitBoardSpawn);
        
        MaxObstaclePossibleSpawnAmount = Mathf.CeilToInt(ObstacleSpawnPercent / 100f * fullBoardSize);
        MaxHeroPossibleSpawnAmount = Mathf.CeilToInt(HeroSpawnPercent / 100f * fullBoardSize);
        MaxMonsterPossibleSpawnAmount = Mathf.CeilToInt(MonsterSpawnPercent / 100f * fullBoardSize);
    }
}
