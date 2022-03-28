using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public GameObject topDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject bottomDoor;
    public GameObject[] spawnPoints;
    public bool isDefault;
    public static bool[,] grid = new bool[5, 7];
    public Vector2Int id;

    private GridObject go;
    private float xSize = 15;
    private float zSize = 10;

    void Start()
    {
        if (isDefault) id = new Vector2Int(2, 0);
        grid[2, 0] = true;
        go = FindObjectOfType<GridObject>();
        DeactivateBadDoorways();
    }

    private void Update()
    {
        RecalculateDoorwaysOnGrid();
    }

    public void DeactivateBadDoorways()
    {
        Vector3 newRoom = topDoor.transform.position + new Vector3(0, 0, zSize);
        Vector2Int roomXY = go.GetGrid().GetXY(newRoom);
        if (go.GetGrid().ContainsCell(roomXY.x, roomXY.y))
        {
            topDoor.SetActive(false);
        }

        newRoom = leftDoor.transform.position - new Vector3(xSize, 0, 0);
        roomXY = go.GetGrid().GetXY(newRoom);
        if (go.GetGrid().ContainsCell(roomXY.x, roomXY.y))
        {
            leftDoor.SetActive(false);
        }

        newRoom = rightDoor.transform.position + new Vector3(xSize, 0, 0);
        roomXY = go.GetGrid().GetXY(newRoom);
        if (go.GetGrid().ContainsCell(roomXY.x, roomXY.y))
        {
            rightDoor.SetActive(false);
        }

        newRoom = bottomDoor.transform.position - new Vector3(0, 0, zSize);
        roomXY = go.GetGrid().GetXY(newRoom);
        if (go.GetGrid().ContainsCell(roomXY.x, roomXY.y))
        {
            bottomDoor.SetActive(false);
        }
    }

    public void DeactivateAllDoorways()
    {
        topDoor.SetActive(true);
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);
        bottomDoor.SetActive(true);
    }

    public Vector2Int GetID()
    {
        return id;
    }

    public void SetID(int x, int y)
    {
        id = new Vector2Int(x, y);
    }

    public void SetGridSpace(int x, int y)
    {
        if (x >= 0 && x < 5 && y >= 0 && y < 7)
        {
            grid[x, y] = true;
        }
    }

    public void RecalculateDoorwaysOnGrid()
    {
        Vector2Int gridXY;
        MapGrid grid = go.GetGrid();

        if (!topDoor.activeSelf)
        {
            gridXY = grid.GetXY(topDoor.transform.position);
            grid.SetValue(gridXY.x, gridXY.y, false);
            grid.SetValue(gridXY.x + 1, gridXY.y, false);
            grid.SetValue(gridXY.x - 1, gridXY.y, false);
            grid.SetValue(gridXY.x - 2, gridXY.y, false);
        }
        if (!bottomDoor.activeSelf)
        {
            gridXY = grid.GetXY(bottomDoor.transform.position);
            grid.SetValue(gridXY.x, gridXY.y, false);
            grid.SetValue(gridXY.x + 1, gridXY.y, false);
            grid.SetValue(gridXY.x + 2, gridXY.y, false);
            grid.SetValue(gridXY.x - 1, gridXY.y, false);
        }
        if (!leftDoor.activeSelf)
        {
            gridXY = grid.GetXY(leftDoor.transform.position);
            grid.SetValue(gridXY.x, gridXY.y, false);
            grid.SetValue(gridXY.x, gridXY.y - 1, false);
            grid.SetValue(gridXY.x, gridXY.y + 1, false);
        }
        if (!rightDoor.activeSelf)
        {
            gridXY = grid.GetXY(rightDoor.transform.position);
            grid.SetValue(gridXY.x, gridXY.y, false);
            grid.SetValue(gridXY.x, gridXY.y - 1, false);
            grid.SetValue(gridXY.x, gridXY.y + 1, false);
        }
    }
}
