using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonRoom : MonoBehaviour
{
    public GameObject topDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject bottomDoor;
    public GameObject[] spawnPoints;
    public bool isDefault;
    public static bool[,] grid = new bool[5, 7];
    public static Vector2Int bossRoomID;
    public static Vector2Int treasureRoomID;
    public Vector2Int id;

    private GridObject go;
    private GameManager gm;

    void Start()
    {
        if (isDefault) id = new Vector2Int(2, 0);
        grid[2, 0] = true;
        go = FindObjectOfType<GridObject>();
        gm = FindObjectOfType<GameManager>();
        Invoke("LateStart", 0.1f);
    }

    void LateStart()
    {
        ActivateGoodDoorways();
    }

    public void ActivateGoodDoorways()
    {
        /*
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
        */
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }

        Vector2Int nextID;
        nextID = id + new Vector2Int(0, 1);
        if (gm.IsValidPosition(nextID.x, nextID.y) && gm.roomGrid[nextID.x, nextID.y] && !DoorwayCutOff(id, nextID))
        {
            topDoor.SetActive(false);
        }

        nextID = id - new Vector2Int(1, 0);
        if (gm.IsValidPosition(nextID.x, nextID.y) && gm.roomGrid[nextID.x, nextID.y] && !DoorwayCutOff(id, nextID))
        {
            leftDoor.SetActive(false);
        }

        nextID = id + new Vector2Int(1, 0);
        if (gm.IsValidPosition(nextID.x, nextID.y) && gm.roomGrid[nextID.x, nextID.y] && !DoorwayCutOff(id, nextID))
        {
            rightDoor.SetActive(false);
        }

        nextID = id - new Vector2Int(0, 1);
        if (gm.IsValidPosition(nextID.x, nextID.y) && gm.roomGrid[nextID.x, nextID.y] && !DoorwayCutOff(id, nextID))
        {
            bottomDoor.SetActive(false);
        }

        Invoke("RecalculateDoorwaysOnGrid", 0.1f);
    }

    public bool DoorwayCutOff(Vector2Int room1, Vector2Int room2)
    {
        foreach (Tuple<Vector2Int, Vector2Int> id in gm.doorsClosed)
        {
            if ((id.Item1.x == room1.x && id.Item1.y == room1.y &&
                id.Item2.x == room2.x && id.Item2.y == room2.y) 
                ||
                (id.Item2.x == room1.x && id.Item2.y == room1.y &&
                id.Item1.x == room2.x && id.Item1.y == room2.y))
            {
                return true;
            }
        }
        return false;
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
        if (!topDoor.activeSelf)
        {
            go.RecalculateDoors(topDoor.transform.position);
        }
        if (!bottomDoor.activeSelf)
        {
            go.RecalculateDoors(bottomDoor.transform.position);
        }
        if (!leftDoor.activeSelf)
        {
            go.RecalculateDoors(leftDoor.transform.position);
        }
        if (!rightDoor.activeSelf)
        {
            go.RecalculateDoors(rightDoor.transform.position);
        }
    }
}
