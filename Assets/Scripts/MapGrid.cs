using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid
{
    private int width;
    private int height;
    private float cellSize;
    private bool[,] gridArray;
    private Vector3 offset;

    public MapGrid(int width, int height, float cellSize, Vector3 offset)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset = offset;

        this.gridArray = new bool[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.red, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
    }

    public bool GetValue(int x, int y)
    {
        return gridArray[x, y];
    }

    public void SetValue(int x, int y, bool value)
    {
        if (x >= 0 && x <= width && y >= 0 && y <= height)
        {
            gridArray[x, y] = value;
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + offset;
    }

    public Vector2Int GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - offset.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.z - offset.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public bool ContainsCell(int x, int y)
    {
        bool existsOnX = (x < width) && (x >= 0);
        bool existsOnY = (y < height) && (y >= 0);
        /*
        if (existsOnX && existsOnY)
        {
            return gridArray[x, y];
        }
        */
        return existsOnX && existsOnY;
    }

    public Vector3 AttemptMove(Vector3 currPos, Vector3 destPos)
    {
        Vector2Int gridCurrPos = GetXY(currPos);
        Vector2Int gridDestPos = GetXY(destPos);
        if (ContainsCell(gridDestPos.x, gridDestPos.y) && !GetValue(gridDestPos.x, gridDestPos.y))
        {
            SetValue(gridCurrPos.x, gridCurrPos.y, false);
            SetValue(gridDestPos.x, gridDestPos.y, true);
            return GetWorldPosition(gridDestPos.x, gridDestPos.y) + new Vector3(cellSize / 2, 0, cellSize / 2);
        }
        return GetWorldPosition(gridCurrPos.x, gridCurrPos.y) + new Vector3(cellSize / 2, 0, cellSize / 2);
    }
}
