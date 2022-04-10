using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DungeonRoom defaultRoom;
    public TreasureBehavior treasure;

    // Room generation
    private DungeonRoom activeRoom;
    private GridObject go;
    private GameObject player;
    public bool[,] roomGrid = new bool[5, 7];
    public List<Tuple<Vector2Int, Vector2Int>> roomPaths = new List<Tuple<Vector2Int, Vector2Int>>();
    private Vector2Int activeGrid;
    private float xSize = 15 + 3;
    private float zSize = 10 + 3;

    // Enemy tracking
    private GameObject[] activeEnemies;
    private bool inNewRoom;
    private RoomCard[] activeTreasureRC;
    private CombatCard[] activeTreasureCC;
    private int activeTreasureNum;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        go = FindObjectOfType<GridObject>();
        activeRoom = defaultRoom;
        activeGrid = new Vector2Int(2, 0);
        roomGrid[2, 0] = true;
        StartGeneratePathways();
    }

    private void Update()
    {
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (inNewRoom)
        {
            activeRoom.DeactivateAllDoorways();
            if (activeEnemies.Length == 0)
            {
                activeRoom.ActivateGoodDoorways();
                if (activeTreasureCC.Length != 0 || activeTreasureRC.Length != 0)
                {
                    int randNum = UnityEngine.Random.Range(0, 8);
                    TreasureBehavior t = Instantiate(treasure, activeRoom.spawnPoints[randNum].transform.position, this.transform.localRotation);
                    t.SetRewards(activeTreasureNum, activeTreasureCC, activeTreasureRC);
                }
                inNewRoom = false;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MakeRoom(DungeonRoom parentRoom, Direction direction, RoomCard rc)
    {
        DungeonRoom dr;
        Vector3 newPos;
        Vector3 playerMoveDir = new Vector3();
        Direction targetDir;

        switch (direction)
        {
            case Direction.UP:
                newPos = parentRoom.gameObject.transform.position + new Vector3(0, 0, zSize);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.DOWN;
                playerMoveDir = new Vector3(0, 0, 2);

                dr.id = parentRoom.GetID() + new Vector2Int(0, 1);
                break;
            case Direction.DOWN:
                newPos = parentRoom.gameObject.transform.position - new Vector3(0, 0, zSize);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.UP;
                playerMoveDir = new Vector3(0, 0, -2);

                dr.id = parentRoom.GetID() - new Vector2Int(0, 1);
                break;
            case Direction.LEFT:
                newPos = parentRoom.gameObject.transform.position - new Vector3(xSize, 0, 0);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.RIGHT;
                playerMoveDir = new Vector3(-2, 0);

                dr.id = parentRoom.GetID() - new Vector2Int(1, 0);
                break;
            case Direction.RIGHT:
                newPos = parentRoom.gameObject.transform.position + new Vector3(xSize, 0, 0);
                dr = Instantiate(defaultRoom, newPos, this.transform.localRotation);
                targetDir = Direction.LEFT;
                playerMoveDir = new Vector3(2, 0);

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
            int randNum = UnityEngine.Random.Range(0, 8);
            if (!usedSpawnPoints.Contains(randNum))
            {
                int randEnemyID = UnityEngine.Random.Range(0, rc.potentialEnemies.Length - 1);
                GameObject randEnemy = rc.potentialEnemies[randEnemyID];
                Instantiate(randEnemy, dr.spawnPoints[randNum].transform.position, randEnemy.transform.localRotation);
                usedSpawnPoints.Add(randNum);
            }
            else
            {
                i--;
            }
        }

        inNewRoom = true;
        player.transform.position = go.GetGrid().AttemptMove(player.transform.position, player.transform.position + playerMoveDir);
        activeTreasureRC = rc.roomCardRewards;
        activeTreasureCC = rc.combatCardRewards;
        activeTreasureNum = rc.numberOfRewards;

        go.RecalculateAvailableSpaces();
        //DisableRedundantTriggers(dr);
    }

    private void StartGeneratePathways()
    {
        int numRooms = 5 * 7 / 2;
        RecursiveGeneratePathways(numRooms, new Vector2Int(2, 0));
    }

    private void RecursiveGeneratePathways(int roomsLeft, Vector2Int currentRoom)
    {
        if (roomsLeft > 0)
        {
            // Debug.Log("[" + currentRoom.x.ToString() + "," + currentRoom.y.ToString() + "]");
            Vector2Int nextRoom = RandomNextStep(GetRandomVacantRoom(3));
            roomGrid[nextRoom.x, nextRoom.y] = true;
            roomPaths.Add(new Tuple<Vector2Int, Vector2Int>(currentRoom, nextRoom));
            RecursiveGeneratePathways(roomsLeft - 1, nextRoom);
        }
    }

    private Vector2Int GetRandomVacantRoom(int preferredVacancies)
    {
        List<int[]> ids = new List<int[]>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (roomGrid[i, j])
                {
                    int[] id = new int[2];
                    id[0] = i;
                    id[1] = j;
                    ids.Add(id);
                }
            }
        }

        List<Vector2Int> idealIDs = new List<Vector2Int>();
        foreach (int[] room_id in ids) {
            Vector2Int vec2intID = new Vector2Int(room_id[0], room_id[1]);
            if (NumAvailableNeighbors(vec2intID) == preferredVacancies)
            {
                idealIDs.Add(vec2intID);
            }
        }

        if (idealIDs.Count > 0)
        {
            int rand_id = UnityEngine.Random.Range(0, idealIDs.Count);
            Vector2Int randRoomID = idealIDs[rand_id];
            return randRoomID;
        }
        else
        {
            return GetRandomVacantRoom(preferredVacancies - 1);
        }

        /*
        int rand_id = UnityEngine.Random.Range(0, ids.Count);
        Vector2Int randRoomID = new Vector2Int(ids[rand_id][0], ids[rand_id][1]);
        if (NumAvailableNeighbors(randRoomID) > 0)
        {
            return randRoomID;
        }
        else
        {
            return GetRandomVacantRoom();
        }
        */
    }

    private int NumAvailableNeighbors(Vector2Int roomID)
    {
        int numRooms = 0;
        Vector2Int nextID;
        nextID = roomID + new Vector2Int(0, 1);
        if (IsValidPosition(nextID.x, nextID.y) && !roomGrid[nextID.x, nextID.y])
        {
            numRooms++;
        }

        nextID = roomID - new Vector2Int(1, 0);
        if (IsValidPosition(nextID.x, nextID.y) && !roomGrid[nextID.x, nextID.y])
        {
            numRooms++;
        }

        nextID = roomID + new Vector2Int(1, 0);
        if (IsValidPosition(nextID.x, nextID.y) && !roomGrid[nextID.x, nextID.y])
        {
            numRooms++;
        }

        nextID = roomID - new Vector2Int(0, 1);
        if (IsValidPosition(nextID.x, nextID.y) && !roomGrid[nextID.x, nextID.y])
        {
            numRooms++;
        }

        return numRooms;
    }

    private Vector2Int RandomNextStep(Vector2Int xy)
    {
        Vector2Int toReturn = new Vector2Int(0, 0);
        int rand = UnityEngine.Random.Range(0, 5);
        switch (rand)
        {
            // UP
            case 0:
                toReturn.x = xy.x;
                toReturn.y = xy.y + 1;
                break;
            // DOWN
            case 1:
                toReturn.x = xy.x;
                toReturn.y = xy.y - 1;
                break;
            // LEFT
            case 2:
                toReturn.x = xy.x - 1;
                toReturn.y = xy.y;
                break;
            // RIGHT
            case 3:
                toReturn.x = xy.x + 1;
                toReturn.y = xy.y;
                break;
        }
        if (IsValidPosition(toReturn.x, toReturn.y) && !roomGrid[toReturn.x, toReturn.y])
        {
            return toReturn;
        }
        else
        {
            return RandomNextStep(xy);
        }
    }

    public bool IsValidPosition(int x, int y)
    {
        return !(x < 0) && !(x > 4) && !(y < 0) && !(y > 6);
    }

    public Vector3 GetRoomWorldPosition()
    {
        return activeRoom.transform.position;
    }

    public void SetActiveRoom(DungeonRoom room)
    {
        activeRoom = room;
    }
}