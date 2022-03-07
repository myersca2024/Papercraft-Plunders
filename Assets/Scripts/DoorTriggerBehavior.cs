using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class DoorTriggerBehavior : MonoBehaviour
{
    public Direction direction;

    private GameManager gm;
    private DungeonRoom parentRoom;
    private bool adjacentRoom = false;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        parentRoom = this.transform.parent.gameObject.GetComponent<DungeonRoom>();
    }

    public void SetAdjacent(bool isAdjacent)
    {
        adjacentRoom = isAdjacent;
    }

    bool IsValidRoom(int x, int y)
    {
        switch (direction)
        {
            case Direction.UP:
                int y2 = y + 1;
                if (x >= 0 && x < 5 && y2 >= 0 && y2 < 7)
                {
                    return !DungeonRoom.grid[x, y2];
                }
                break;
            case Direction.DOWN:
                int y3 = y - 1;
                if (x >= 0 && x < 5 && y3 >= 0 && y3 < 7)
                {
                    return !DungeonRoom.grid[x, y3];
                }
                break;
            case Direction.LEFT:
                int x2 = x - 1;
                if (x2 >= 0 && x2 < 5 && y >= 0 && y < 7)
                {
                    return !DungeonRoom.grid[x2, y];
                }
                break;
            case Direction.RIGHT:
                int x3 = x + 1;
                if (x3 >= 0 && x3 < 5 && y >= 0 && y < 7)
                {
                    return !DungeonRoom.grid[x3, y];
                }
                break;
            default:
                break;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector2Int id = parentRoom.GetID();
            Debug.Log(id.ToString());
            if (!adjacentRoom && IsValidRoom(id.x, id.y))
            {
                gm.MakeRoom(parentRoom, direction);
                adjacentRoom = true;
            }
        }
    }
}
