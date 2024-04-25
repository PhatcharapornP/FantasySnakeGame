using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform bgParent;
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] [Min(35)] private int pieceSize = 35;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float widthDiff;
    [SerializeField] private float heightDiff;
    
    [SerializeField] private List<GameObject> spawnedBG = new List<GameObject>();
    [SerializeField] private List<Hero> spawnedHero = new List<Hero>();
    [SerializeField] private List<Monster> spawnedMonster = new List<Monster>();
    

    public void GenerateBoard()
    {
        CalculatePieceSize();
        CalculatePositionOffset();
        
        
        SpawnBackground();
    }
    
    private void CalculatePieceSize()
    {
        int rows = GameManager.Instance.Tweaks.Board_Row_Size;
        int columns = GameManager.Instance.Tweaks.Board_Column_Size;
        var cellWidth = targetRectTransform.rect.width / columns - ((spacing / columns) * 2);
        var cellHeight = targetRectTransform.rect.height / rows - ((spacing / rows) * 2);

        if (cellWidth < cellHeight)
            pieceSize = Mathf.FloorToInt(cellWidth);
        else
            pieceSize = Mathf.FloorToInt(cellHeight);

        var totalColumnAvailable = Mathf.FloorToInt(targetRectTransform.rect.width / pieceSize);
        var totalRowsAvailable = Mathf.FloorToInt(targetRectTransform.rect.height / pieceSize);

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

        positionOffset = targetRectTransform.position - center;
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
        }
    }
}
