using System.Collections;
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
    private bool[,] roomGrid = new bool[5, 7];
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
    }

    private void Update()
    {
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (inNewRoom)
        {
            activeRoom.DeactivateAllDoorways();
            if (activeEnemies.Length == 0)
            {
                activeRoom.DeactivateBadDoorways();
                if (activeTreasureCC.Length != 0 || activeTreasureRC.Length != 0)
                {
                    int randNum = Random.Range(0, 8);
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
            int randNum = Random.Range(0, 8);
            if (!usedSpawnPoints.Contains(randNum))
            {
                int randEnemyID = Random.Range(0, rc.potentialEnemies.Length - 1);
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

    public Vector3 GetRoomWorldPosition()
    {
        return activeRoom.transform.position;
    }

    public void SetActiveRoom(DungeonRoom room)
    {
        activeRoom = room;
    }
}