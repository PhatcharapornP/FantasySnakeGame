using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private RectTransform bgParent;
    [SerializeField] private RectTransform unitParent;
    [SerializeField] private RectTransform playerUnitParent;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] [Min(35)] private int pieceSize = 35;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float widthDiff;
    [SerializeField] private float heightDiff;
    [SerializeField] private List<BaseBoardUnit> spawnedHero = new List<BaseBoardUnit>();
    [SerializeField] private List<BaseBoardUnit> spawnedMonster = new List<BaseBoardUnit>();
    [SerializeField] private List<ObstacleSpawnData> spawnObstacle = new List<ObstacleSpawnData>();
    [SerializeField] private List<BaseBoardUnit> spawnedBG = new List<BaseBoardUnit>();
    [SerializeField] private Vector2Int obstacleSpec;
    private List<Vector2Int> playerFreePos = new List<Vector2Int>();
    private Dictionary<Vector2Int, BaseBoardUnit> board = new Dictionary<Vector2Int, BaseBoardUnit>();
    
    private void Clearboard()
    {
        foreach (var BG in spawnedBG)
        {
            BG.RemoveUnitFromBoard();
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
        spawnedHero.Clear();
        spawnedMonster.Clear();
        spawnObstacle.Clear();
        board.Clear();
    }

    public void GenerateBoard()
    {
        Clearboard();
        GameManager.Instance.Tweaks.SetupSpawnPossibleAmount();
        CalculatePieceSize();
        CalculatePositionOffset();
        SpawnBoardPiece();
        SpawnBackground();
    }

    public void SpawnBoardPiece()
    {
        PresetBoardSpawnOnGameStart();
    }

    
    public void PresetBoardSpawnOnGameStart()
    {
        int playerColumn = SpawnFirstPartyLeader();
        //TODO: Spawn ground around player --> give breathing space at the start of the game
        playerFreePos.Add(new Vector2Int(playerColumn+1,0));
        playerFreePos.Add(new Vector2Int(playerColumn+1,0));
        playerFreePos.Add(new Vector2Int(playerColumn-1,0));
        playerFreePos.Add(new Vector2Int(playerColumn,1));
        playerFreePos.Add(new Vector2Int(playerColumn-1,1));
        playerFreePos.Add(new Vector2Int(playerColumn+1,1));

        Vector2Int tmpRandomPos = new Vector2Int();
        bool isFreeBoardPos = false;
        int mostFreePosAttempPossible = 0;
        int tmpFindFreePosAttemp = 0;
        int maxUnitOnBoardAmount = GameManager.Instance.Tweaks.Board_Column_Size *GameManager.Instance.Tweaks.Board_Row_Size;
        int unitOnBoardAmount = 0;
        
        //TODO: Spawn obstacle
        mostFreePosAttempPossible = maxUnitOnBoardAmount - unitOnBoardAmount;
        while (spawnObstacle.Count < GameManager.Instance.Tweaks.MaxObstaclePossibleSpawnAmount && tmpFindFreePosAttemp < mostFreePosAttempPossible)
        {
            while (!isFreeBoardPos && tmpFindFreePosAttemp < mostFreePosAttempPossible)
            {
                tmpRandomPos.x = Random.Range(0, GameManager.Instance.Tweaks.Board_Column_Size);
                tmpRandomPos.y = Random.Range(0, GameManager.Instance.Tweaks.Board_Row_Size);

                if (board.ContainsKey(tmpRandomPos) == false && playerFreePos.Contains(tmpRandomPos) == false)
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
                if (IsAbleToSpawnObstacleSpec(tmpRandomPos))
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
        tmpFindFreePosAttemp = 0;
        while (spawnedHero.Count < GameManager.Instance.Tweaks.MinHeroPossibleSpawnAmount && tmpFindFreePosAttemp < mostFreePosAttempPossible)
        {
            //Oh boi
            while (!isFreeBoardPos && tmpFindFreePosAttemp < mostFreePosAttempPossible)
            {
                tmpRandomPos.x = Random.Range(0, GameManager.Instance.Tweaks.Board_Column_Size);
                tmpRandomPos.y = Random.Range(0, GameManager.Instance.Tweaks.Board_Row_Size);

                if (board.ContainsKey(tmpRandomPos) == false&& playerFreePos.Contains(tmpRandomPos) == false)
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
        while (spawnedMonster.Count < GameManager.Instance.Tweaks.MinMonsterPossibleSpawnAmount && tmpFindFreePosAttemp < mostFreePosAttempPossible)
        {
            //Oh boi
            while (!isFreeBoardPos && tmpFindFreePosAttemp < mostFreePosAttempPossible)
            {
                tmpRandomPos.x = Random.Range(0, GameManager.Instance.Tweaks.Board_Column_Size );
                tmpRandomPos.y = Random.Range(0, GameManager.Instance.Tweaks.Board_Row_Size );

                if (board.ContainsKey(tmpRandomPos) == false&& playerFreePos.Contains(tmpRandomPos) == false)
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
    }

    private bool IsAbleToSpawnObstacleSpec(Vector2Int targetPos)
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
                return false;
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

        if (partyLeader.transform.parent != playerUnitParent)
            partyLeader.transform.SetParent(playerUnitParent);

        int playerColumn = (GameManager.Instance.Tweaks.Board_Column_Size / 2) - 1;

        SetupUnitPositionData(new Vector2Int(playerColumn,0),partyLeader);
        partyLeader.OnUnitSpawnOnBoard();
        Player.Instance.AddHeroToPlayerParty((Hero)partyLeader);
        board.Add(partyLeader.BoardPosition, partyLeader);
        partyLeader.SetupUnitTrasnformOnScreen();
        spawnedHero.Add(partyLeader);
        return playerColumn;
    }
    
    private void SpawnHero(int column, int row)
    {
        var hero = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Hero);
        if (hero.transform.parent != playerUnitParent)
            hero.transform.SetParent(playerUnitParent);

        SetupUnitPositionData(new Vector2Int(column,row),hero);
        board.Add(hero.BoardPosition, hero);
        hero.OnUnitSpawnOnBoard();
        spawnedHero.Add((Hero)hero);
        hero.SetupUnitTrasnformOnScreen();
    }
    
    private void SpawnMonster(int column, int row)
    {
        var monster = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Monster);
        if (monster.transform.parent != unitParent)
            monster.transform.SetParent(unitParent);

        SetupUnitPositionData(new Vector2Int(column,row),monster);
        board.Add(monster.BoardPosition, monster);
        monster.OnUnitSpawnOnBoard();
        spawnedMonster.Add((Monster)monster);
        monster.SetupUnitTrasnformOnScreen();
    }

    private void SpawnObstacle( int column, int row)
    {
        if (board.ContainsKey(new Vector2Int(column, row)))
            return;
        var obstacle = GameManager.Instance.Pool.PickFromPool(Globals.PoolType.Obstacle);
        if (obstacle.transform.parent != unitParent)
            obstacle.transform.SetParent(unitParent);

        SetupUnitPositionData(new Vector2Int(column,row),obstacle);
        board.Add(obstacle.BoardPosition, obstacle);
        obstacle.OnUnitSpawnOnBoard();
        spawnObstacle.Add(new ObstacleSpawnData()
        {
            Spec = obstacleSpec,
            Unit = (Obstacle)obstacle
        });
        obstacle.SetupUnitTrasnformOnScreen();
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

    public void SetupUnitPositionData(Vector2Int boardPos, BaseBoardUnit unit)
    {
        unit.BoardPosition = boardPos;
        unit.GameobjPosition = new Vector3(pieceSize * unit.BoardPosition.x,pieceSize * unit.BoardPosition.y,0) - positionOffset;
        unit.GameobjScale = new Vector3(pieceSize - spacing, pieceSize, 1);
    }

    public Vector3 GetGameObjPos(Vector2Int boardPos)
    {
        return new Vector3(pieceSize * boardPos.x,pieceSize * boardPos.y,0) - positionOffset;
    }

    public void UpdateUnitPosOnBoard(Vector2Int pos,Vector2Int previousPos,BaseBoardUnit unit)
    {
        board.Remove(previousPos);
        if (board.TryGetValue(pos,out var tmp) && tmp!= null)
            board[pos] = unit;
        else
            board.Add(pos,unit);
    }
    
    public BaseBoardUnit GetBoardUnitFromPos(Vector2Int targetPos)
    {
        if (board.ContainsKey(targetPos))
            return board[targetPos];
        return null;
    }

    public void CompletelyRemoveFromBoard(BaseBoardUnit unit)
    {
        switch (unit.UnitType)
        {
            case Globals.PoolType.Hero:
                spawnedHero.Remove(unit);
                if (spawnedHero.Count < 5)
                    FillInRandomGroundWithNewHero();
                break;
            case Globals.PoolType.Monster:
                spawnedMonster.Remove((Monster)unit);
                if (spawnedMonster.Count < 5)
                    FillInRandomGroundWithNewMonster();
                break;
            case Globals.PoolType.Obstacle:
            {
                var tmp = spawnObstacle.Find(x => x.Unit == unit);
                spawnObstacle.Remove(tmp);
            }
                break;
        }
        board.Remove(unit.BoardPosition);
        unit.RemoveUnitFromBoard();
    }
    
    public void FillInRandomGroundWithNewHero()
    {
        if (spawnedHero.Count+1 > GameManager.Instance.Tweaks.MaxHeroPossibleSpawnAmount)
            return;
            
        int spawnHeroRNG = Random.Range(0, 2); // 1 in 2 chance
        if (spawnHeroRNG == 0)
        {
            List<Vector2Int> freeSpotList = new List<Vector2Int>();
            Vector2Int tmpPos = Vector2Int.zero;;
            for (int c = 0; c < GameManager.Instance.Tweaks.Board_Column_Size; c++)
            {
                tmpPos.x = c;
                for (int r = 0; r < GameManager.Instance.Tweaks.Board_Row_Size; r++)
                {
                    tmpPos.y = r;
                    if (board.ContainsKey(tmpPos) == false)
                        freeSpotList.Add(tmpPos);
                }
            }

            int rngGround = Random.Range(0, freeSpotList.Count - 1);
            var targetGroundPos = freeSpotList[rngGround];
            SpawnHero(targetGroundPos.x,targetGroundPos.y);
        }
    }
    
    public void FillInRandomGroundWithNewMonster()
    {
        if (spawnedMonster.Count+1 > GameManager.Instance.Tweaks.MaxMonsterPossibleSpawnAmount)
            return;
        int spawnMonsterRNG = Random.Range(0, 2); // 1 in 2 chance
        if (spawnMonsterRNG == 0)
        {
            List<Vector2Int> freeSpotList = new List<Vector2Int>();
            Vector2Int tmpPos = Vector2Int.zero;;
            for (int c = 0; c < GameManager.Instance.Tweaks.Board_Column_Size; c++)
            {
                tmpPos.x = c;
                for (int r = 0; r < GameManager.Instance.Tweaks.Board_Row_Size; r++)
                {
                    tmpPos.y = r;
                    if (board.ContainsKey(tmpPos) == false)
                        freeSpotList.Add(tmpPos);
                }
            }

            int rngGround = Random.Range(0, freeSpotList.Count - 1);
            var targetGroundPos = freeSpotList[rngGround];
            SpawnMonster(targetGroundPos.x,targetGroundPos.y);
        }
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

    #endregion
}

[Serializable]
public class ObstacleSpawnData
{
    public Vector2Int Spec;
    public BaseBoardUnit Unit;
}