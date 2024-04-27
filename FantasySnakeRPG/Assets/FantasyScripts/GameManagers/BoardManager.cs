using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private RectTransform bgParent;
    [SerializeField] private RectTransform unitParent;
    [SerializeField] private RectTransform groundParent;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] [Min(35)] private int pieceSize = 35;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float widthDiff;
    [SerializeField] private float heightDiff;

    private Dictionary<Vector2Int, BaseBoardUnit> board = new Dictionary<Vector2Int, BaseBoardUnit>();

    [SerializeField] private List<Hero> spawnedHero = new List<Hero>();

    [SerializeField] private List<Monster> spawnedMonster = new List<Monster>();

    [SerializeField] private List<ObstacleSpawnData> spawnObstacle = new List<ObstacleSpawnData>();
    [SerializeField] private List<BaseBoardUnit> spawnedBG = new List<BaseBoardUnit>();
    [SerializeField] private List<BaseBoardUnit> spawnedGround = new List<BaseBoardUnit>();
    [SerializeField] private Vector2Int obstacleSpec;

    private List<KeyValuePair<Globals.PoolType, int>> SpawnWeightPairList = new List<KeyValuePair<Globals.PoolType, int>>();

    public void Clearboard()
    {
        foreach (var BG in spawnedBG)
        {
            BG.RemoveUnitFromBoard();
        }
        
        foreach (var ground in spawnedGround)
        {
            ground.RemoveUnitFromBoard();
        }

        foreach (var unit in spawnedHero)
        {
            unit.RemoveUnitFromBoard();
        }

        foreach (var unit in spawnedMonster)
        {
            unit.RemoveUnitFromBoard();
        }

        foreach (var data in spawnObstacle)
        {
            data.Unit.RemoveUnitFromBoard();
        }
        spawnedBG.Clear();
        spawnedGround.Clear();
        spawnedHero.Clear();
        spawnedMonster.Clear();
        spawnObstacle.Clear();
        SpawnWeightPairList.Clear();
        board.Clear();
        SpawnWeightPairList = GameManager.Instance.Tweaks.GetSpawnWeightPairList();
    }

    public void GenerateBoard()
    {
        Clearboard();
        SpawnWeightPairList = GameManager.Instance.Tweaks.GetSpawnWeightPairList();
        GameManager.Instance.Tweaks.SetupSpawnPossibleAmount();
        CalculatePieceSize();
        CalculatePositionOffset();
        SpawnBoardPiece();
        SpawnBackground();
    }

    public void SpawnBoardPiece()
    {
        PresetBoardSpawnOnGameStart();
        foreach (var boardUnitPair in board)
        {
            SetupUnitTransform(boardUnitPair.Value);
        }
    }

    public void PresetBoardSpawnOnGameStart()
    {
        int playerColumn = SpawnFirstPartyLeader();
        //TODO: Spawn ground around player --> give breathing space at the start of the game
        SpawnGround(playerColumn+1,0);
        SpawnGround(playerColumn-1,0);
        SpawnGround(playerColumn,1);

        Vector2Int tmpRandomPos = new Vector2Int();
        bool isFreeBoardPos = false;
        int mostFreePosAttempPossible = 0;
        int tmpFindFreePosAttemp = 0;
        int maxUnitOnBoardAmount = GameManager.Instance.Tweaks.Board_Column_Size *GameManager.Instance.Tweaks.Board_Row_Size;
        int unitOnBoardAmount = 0;
        
        //TODO: Spawn obstacle
        mostFreePosAttempPossible = maxUnitOnBoardAmount - unitOnBoardAmount;
        Debug.Log($"On try to spawn obstacle mostFreePosAttempPossible: {mostFreePosAttempPossible} | maxUnitOnBoardAmount: {maxUnitOnBoardAmount} | unitOnBoardAmount: {unitOnBoardAmount}".InColor(new Color(1f, 0.63f, 0.82f)));
        while (spawnObstacle.Count < GameManager.Instance.Tweaks.obstaclePossibleSpawnAmount && tmpFindFreePosAttemp < mostFreePosAttempPossible)
        {
            
            //Oh boi
            while (!isFreeBoardPos && tmpFindFreePosAttemp < mostFreePosAttempPossible)
            {
                tmpRandomPos.x = Random.Range(0, GameManager.Instance.Tweaks.Board_Column_Size);
                tmpRandomPos.y = Random.Range(0, GameManager.Instance.Tweaks.Board_Row_Size);

                if (board.ContainsKey(tmpRandomPos) == false)
                {
                    isFreeBoardPos = true;
                    tmpFindFreePosAttemp = 0;
                  
                }
                else
                    tmpFindFreePosAttemp++;
            }

            if (isFreeBoardPos)
            {
                obstacleSpec = new Vector2Int(Random.Range(1, 3), Random.Range(1, 3));
                //TODO: Check if obstacle is affoardable...
                if (IsFreeForObstacleSpec(tmpRandomPos))
                {
                    for (int x = 0; x < obstacleSpec.x; x++)
                    {
                        //Debug.Log($"spawn x at: column: {tmpRandomPos.x+x},row: {tmpRandomPos.y}".InColor(new Color(1f, 0.38f, 0.91f)));
                        SpawnObstacle(tmpRandomPos.x+x,tmpRandomPos.y);
                        unitOnBoardAmount++;
                    }

                    for (int y = 0; y < obstacleSpec.y; y++)
                    {
                        //Debug.Log($"spawn x at: column: {tmpRandomPos.x+y},row: {tmpRandomPos.y+1}".InColor(new Color(0.58f, 1f, 0.77f)));
                        SpawnObstacle(tmpRandomPos.x + y,tmpRandomPos.y + 1 );
                        unitOnBoardAmount++;
                    }
                }
                
                
                isFreeBoardPos = false;
            }
            
        }
        
        //TODO: Spawn hero
        mostFreePosAttempPossible = maxUnitOnBoardAmount - unitOnBoardAmount;
        Debug.Log($"On try to spawn hero mostFreePosAttempPossible: {mostFreePosAttempPossible} | maxUnitOnBoardAmount: {maxUnitOnBoardAmount} | unitOnBoardAmount: {unitOnBoardAmount} | tmpFindFreePosAttemp: {tmpFindFreePosAttemp}".InColor(new Color(0.67f, 0.9f, 1f)));
        tmpFindFreePosAttemp = 0;
        
        while (spawnedHero.Count < GameManager.Instance.Tweaks.heroPossibleSpawnAmount && tmpFindFreePosAttemp < mostFreePosAttempPossible)
        {
            //Oh boi
            while (!isFreeBoardPos && tmpFindFreePosAttemp < mostFreePosAttempPossible)
            {
                tmpRandomPos.x = Random.Range(0, GameManager.Instance.Tweaks.Board_Column_Size);
                tmpRandomPos.y = Random.Range(0, GameManager.Instance.Tweaks.Board_Row_Size);

                if (board.ContainsKey(tmpRandomPos) == false)
                {
                    isFreeBoardPos = true;
                    tmpFindFreePosAttemp = 0;
                  
                }
                else
                    tmpFindFreePosAttemp++;
            }

            if (isFreeBoardPos)
            {
                SpawnHero(tmpRandomPos.x,tmpRandomPos.y);
                isFreeBoardPos = false;
            }
            
        }
        
        //TODO: Spawn monster
        mostFreePosAttempPossible = maxUnitOnBoardAmount - unitOnBoardAmount;
        tmpFindFreePosAttemp = 0;
        Debug.Log($"On try to spawn monster mostFreePosAttempPossible: {mostFreePosAttempPossible} | maxUnitOnBoardAmount: {maxUnitOnBoardAmount} | unitOnBoardAmount: {unitOnBoardAmount} | tmpFindFreePosAttemp: {tmpFindFreePosAttemp}".InColor(new Color(0.84f, 1f, 0.76f)));
        while (spawnedMonster.Count < GameManager.Instance.Tweaks.monsterPossibleSpawnAmount && tmpFindFreePosAttemp < mostFreePosAttempPossible)
        {
            //Oh boi
            while (!isFreeBoardPos && tmpFindFreePosAttemp < mostFreePosAttempPossible)
            {
                tmpRandomPos.x = Random.Range(0, GameManager.Instance.Tweaks.Board_Column_Size );
                tmpRandomPos.y = Random.Range(0, GameManager.Instance.Tweaks.Board_Row_Size );

                if (board.ContainsKey(tmpRandomPos) == false)
                {
                    isFreeBoardPos = true;
                    tmpFindFreePosAttemp = 0;
                  
                }
                else
                    tmpFindFreePosAttemp++;
            }

            if (isFreeBoardPos)
            {
                SpawnMonster(tmpRandomPos.x,tmpRandomPos.y);
                isFreeBoardPos = false;
            }
            
        }
        
        //TODO: Fill free space with ground
        for (int column = 0; column < GameManager.Instance.Tweaks.Board_Column_Size; column++)
        for (int row = 0; row < GameManager.Instance.Tweaks.Board_Row_Size; row++)
        {
            if (board.ContainsKey(new Vector2Int(column, row))) continue;
            SpawnGround(column,row);
        }
    }

    private bool IsFreeForObstacleSpec(Vector2Int targetPos)
    {
        if (board.TryGetValue(new Vector2Int(targetPos.x, targetPos.y - 1), out var unitLeft) && unitLeft.UnitType is Globals.PoolType.Obstacle ||
            board.TryGetValue(new Vector2Int(targetPos.x - 1, targetPos.y), out var unitBelow) && unitBelow.UnitType is Globals.PoolType.Obstacle)
            return false;
        
        
        Vector2Int tmpPos = targetPos;
        bool isAbleToAffordSpecX = false;
        List<Vector2Int> specPos = new List<Vector2Int>();
        for (int i = 0; i < obstacleSpec.x; i++)
        {
            tmpPos.x += i;
            specPos.Add(tmpPos);
        }
        
        tmpPos.y += 1;
        for (int i = 0; i < obstacleSpec.y; i++)
        {
            tmpPos.x += i;
            specPos.Add(tmpPos);
        }

        foreach (var pos in specPos)
        {
            if (board.ContainsKey(pos))
            {
                return false;
            }
            else
            {
                if (pos.x > GameManager.Instance.Tweaks.Board_Column_Size-1)
                    return false;
                
                if (pos.y > GameManager.Instance.Tweaks.Board_Row_Size-1)
                    return false;
            }
        }

        return true;

    }

    #region SpawnFunctions
    private int SpawnFirstPartyLeader()
    {
        var partyLeader = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Hero);

        if (partyLeader.transform.parent != unitParent)
            partyLeader.transform.SetParent(unitParent);

        int playerColumn = (GameManager.Instance.Tweaks.Board_Column_Size / 2) - 1;

        SetupUnityBoardPositionData(new Vector2Int(playerColumn,0),new Vector3(pieceSize * playerColumn,pieceSize * 0,0),partyLeader);
        partyLeader.OnUnitSpawnOnBoard();
        Player.Instance.OnBoardCreation((Hero)partyLeader);
        return playerColumn;
    }

    private void SpawnGround(int column, int row)
    {
        var ground = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Ground);
        if (ground.transform.parent != groundParent)
            ground.transform.SetParent(groundParent);

        SetupUnityBoardPositionData(new Vector2Int(column,row),new Vector3(pieceSize * column,pieceSize * row,0),ground);
        ground.OnUnitSpawnOnBoard();
        spawnedGround.Add(ground);
    }
    
    private void SpawnHero(int column, int row)
    {
        var hero = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Hero);
        if (hero.transform.parent != groundParent)
            hero.transform.SetParent(groundParent);

        SetupUnityBoardPositionData(new Vector2Int(column,row),new Vector3(pieceSize * column,pieceSize * row,0),hero);
        hero.OnUnitSpawnOnBoard();
        spawnedHero.Add((Hero)hero);
    }
    
    private void SpawnMonster(int column, int row)
    {
        var monster = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Monster);
        if (monster.transform.parent != groundParent)
            monster.transform.SetParent(groundParent);

        SetupUnityBoardPositionData(new Vector2Int(column,row),new Vector3(pieceSize * column,pieceSize * row,0),monster);
        monster.OnUnitSpawnOnBoard();
        spawnedMonster.Add((Monster)monster);
    }

    private void SpawnObstacle( int column, int row)
    {
        if (board.ContainsKey(new Vector2Int(column, row)))
            return;
        var obstacle = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Obstacle);
        if (obstacle.transform.parent != unitParent)
            obstacle.transform.SetParent(unitParent);

        SetupUnityBoardPositionData(new Vector2Int(column,row),new Vector3(pieceSize * column,pieceSize * row,0),obstacle);
        obstacle.OnUnitSpawnOnBoard();
        spawnObstacle.Add(new ObstacleSpawnData()
        {
            Spec = obstacleSpec,
            Unit = (Obstacle)obstacle
        });
    }
    
    public void SpawnBackground()
    {
        for (int column = 0; column < GameManager.Instance.Tweaks.Board_Column_Size; column++)
        for (int row = 0; row < GameManager.Instance.Tweaks.Board_Row_Size; row++)
        {
            var bg = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.BG);
            if (bg == null)
            {
                Debug.LogError($"tried to get obj from pool with null Piece class!".InColor(Color.red), gameObject);
                break;
            }

            spawnedBG.Add(bg);
            if (bg.transform.parent != bgParent)
                bg.transform.SetParent(bgParent);

            var posX = (pieceSize * column);
            var posY = (pieceSize * row);

            bg.transform.localPosition = new Vector3(posX, posY, 0) - positionOffset;
            bg.transform.localScale = new Vector3(pieceSize - spacing, pieceSize, 1);
            bg.OnUnitSpawnOnBoard();
        }
    }

    public void SetupUnitTransform(BaseBoardUnit boardUnit)
    {
        boardUnit.transform.localPosition = boardUnit.GameobjPosition;
        boardUnit.transform.localScale = boardUnit.GameobjScale;
    }

    public void SetupUnityBoardPositionData(Vector2Int boardPos,Vector3 objPos, BaseBoardUnit unit)
    {
        unit.BoardPosition = boardPos;
        unit.GameobjPosition = objPos - positionOffset;
        unit.GameobjScale = new Vector3(pieceSize - spacing, pieceSize, 1);
     //   Debug.Log($"add {unit.BoardPosition} to board".InColor(Color.red),unit);
        board.Add(unit.BoardPosition, unit);
    }

   
    #endregion

    #region CalculateBoardPositionOffset

    private void CalculatePieceSize()
    {
        int rows = GameManager.Instance.Tweaks.Board_Row_Size;
        int columns = GameManager.Instance.Tweaks.Board_Column_Size;
        var cellWidth = unitParent.rect.width / columns - ((spacing / columns) * 2);
        var cellHeight = unitParent.rect.height / rows - ((spacing / rows) * 2);

        if (cellWidth < cellHeight)
            pieceSize = Mathf.FloorToInt(cellWidth);
        else
            pieceSize = Mathf.FloorToInt(cellHeight);

        var totalColumnAvailable = Mathf.FloorToInt(unitParent.rect.width / pieceSize);
        var totalRowsAvailable = Mathf.FloorToInt(unitParent.rect.height / pieceSize);

        heightDiff = 0;
        heightDiff = totalRowsAvailable - GameManager.Instance.Tweaks.Board_Row_Size;

        widthDiff = 0;
        widthDiff = totalColumnAvailable - columns;
    }

    private void CalculatePositionOffset()
    {
        positionOffset = Vector3.zero;
        Vector3 center = Vector3.zero;

        if (heightDiff > 0)
        {
            center.y = heightDiff * pieceSize / 2.0f;
            center.y += ((pieceSize - spacing) * heightDiff) / 2.0f;
        }

        if (widthDiff > 0)
            center.x = widthDiff * pieceSize / 2.0f;

        positionOffset = unitParent.position - center;
    }

    #endregion

    #region CalculateWeight

    private int MaxWeight(List<KeyValuePair<Globals.PoolType, int>> pairList)
    {
        int maxWeight = 0;

        foreach (var pair in pairList)
        {
            maxWeight += pair.Value;
        }

        return maxWeight;
    }


    public Globals.PoolType GetWeightedRandomPieceType(bool filterObstacle = false)
    {
        //Check if spawn enough monster
        if (spawnedMonster.Count >= GameManager.Instance.Tweaks.monsterPossibleSpawnAmount)
        {
            int index = SpawnWeightPairList.FindIndex(x => x.Key is Globals.PoolType.Monster);
            if (index >= 0)
                SpawnWeightPairList.RemoveAt(index);
        }
        
        if (spawnObstacle.Count >= GameManager.Instance.Tweaks.obstaclePossibleSpawnAmount)
        {
            int index = SpawnWeightPairList.FindIndex(x => x.Key is Globals.PoolType.Obstacle);
            if (index >= 0)
                SpawnWeightPairList.RemoveAt(index);
        }
        
        if (spawnedHero.Count >= GameManager.Instance.Tweaks.heroPossibleSpawnAmount)
        {
            int index = SpawnWeightPairList.FindIndex(x => x.Key is Globals.PoolType.Hero);
            if (index >= 0)
                SpawnWeightPairList.RemoveAt(index);
        }
        
        int maxWeight = MaxWeight(SpawnWeightPairList);
        var poolType = Globals.PoolType.Ground;
        if (maxWeight > 0)
        {
            int tempResult = Random.Range(1, maxWeight);

            foreach (var pair in SpawnWeightPairList)
            {
                if (filterObstacle && (pair.Key is Globals.PoolType.Obstacle))
                    continue;

                if (tempResult <= pair.Value)
                {
                    poolType = pair.Key;
                    break;
                }

                tempResult -= pair.Value;
            }
        }

        if (poolType is Globals.PoolType.Obstacle)
            obstacleSpec = new Vector2Int(Random.Range(1, 3), Random.Range(1, 3));

        return poolType;
    }

    #endregion
}

[Serializable]
public class ObstacleSpawnData
{
    public Vector2Int Spec;
    public Obstacle Unit;
}