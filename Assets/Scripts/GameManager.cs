using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DungeonRoom defaultRoom;

    private DungeonRoom activeRoom;
    private GridObject go;
    private bool[,] roomGrid = new bool[5, 7];
    private Vector2Int activeGrid;
    private float xSize = 15 + 3;
    private float zSize = 10 + 3;

    void Start()
    {
        go = FindObjectOfType<GridObject>();
        activeGrid = new Vector2Int(2, 0);
        roomGrid[2, 0] = true;
    }

    private void Update()
    {
        string s = "";
        for (int i = 6; i >= 0; i--)
        {
            for (int j = 0; j < 5; j++)
            {
                s += (DungeonRoom.grid[j, i].ToString() + " ");
            }
            s += "\n";
        }
        // Debug.Log(s);
    }

    public void Restart()
    {
        // Need to fill this out at some point
    }

    public void MakeRoom(DungeonRoom parentRoom, Direction direction, RoomCard rc)
    {
        DungeonRoom dr;
        Vector3 newPos;
        Direction targetDir;

        switch (direction)
        {
            case Direction.UP:
                newPos = parentRoom.gameObject.transform.position + new Vector3(0, 0, zSize);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.DOWN;

                dr.id = parentRoom.GetID() + new Vector2Int(0, 1);
                break;
            case Direction.DOWN:
                newPos = parentRoom.gameObject.transform.position - new Vector3(0, 0, zSize);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.UP;

                dr.id = parentRoom.GetID() - new Vector2Int(0, 1);
                break;
            case Direction.LEFT:
                newPos = parentRoom.gameObject.transform.position - new Vector3(xSize, 0, 0);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.RIGHT;

                dr.id = parentRoom.GetID() - new Vector2Int(1, 0);
                break;
            case Direction.RIGHT:
                newPos = parentRoom.gameObject.transform.position + new Vector3(xSize, 0, 0);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.LEFT;

                dr.id = parentRoom.GetID() + new Vector2Int(1, 0);
                break;
            default:
                Debug.Log("OH NO! IT ALL WENT WRONG!");
                dr = Instantiate(defaultRoom, this.transform.position, this.transform.localRotation);
                targetDir = direction;
                break;
        }

        activeRoom = dr;

        Vector2Int id = dr.GetID();
        dr.SetGridSpace(id.x, id.y);

        DoorTriggerBehavior[] doors = dr.gameObject.GetComponentsInChildren<DoorTriggerBehavior>();
        foreach (DoorTriggerBehavior door in doors)
        {
            if (door.direction == targetDir) door.SetAdjacent(true);
        }

        List<int> usedSpawnPoints = new List<int>();
        for (int i = 0; i < rc.numberOfEnemies; i++)
        {
            int randNum = Random.Range(0, 8);
            if (!usedSpawnPoints.Contains(randNum))
            {
                int randEnemyID = Random.Range(0, rc.potentialEnemies.Length - 1);
                GameObject randEnemy = rc.potentialEnemies[randEnemyID];
                Instantiate(randEnemy, dr.spawnPoints[randNum].transform.position, this.transform.localRotation);
                usedSpawnPoints.Add(randNum);
            }
            else
            {
                i--;
            }
        }

        go.RecalculateAvailableSpaces();
        //DisableRedundantTriggers(dr);
    }
}