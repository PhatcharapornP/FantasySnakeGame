using System;
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
    
    [Header("Unit count")]
    public int MinUnitSpawnCount = 64;
    public int MaxUnitSpawnCount = 256;
    [Header("Hero spawn")]
    public int MinHeroSpawn = 10;
    public int MaxHeroSpawn = 21;
    public int WeightHeroSpawn = 1;
    [Header("Monster spawn")]
    public int MinMonsterSpawn = 10;
    public int MaxMonsterSpawn = 21;
    public int WeightMonsterSpawn = 2;
    [Header("Obstacle spawn")]
    public int MinObstacleSpawn = 10;
    public int MaxObstacleSpawn = 21;
    public int WeightObstacleSpawn = 1;
    
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
    
    
}
