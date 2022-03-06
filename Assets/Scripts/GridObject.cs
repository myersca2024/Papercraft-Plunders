using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public HitDetector hitDetector;

    private MapGrid grid;
    private GameObject player;

    void Awake()
    {
        grid = new MapGrid(width, height, cellSize, this.transform.position);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RecalculateAvailableSpaces();
    }

    public void RecalculateAvailableSpaces()
    {
        Vector2Int playerPos = grid.GetXY(player.transform.position);
        int buffer = 20;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((Mathf.Abs(playerPos.x + i) <= buffer || Mathf.Abs(playerPos.x - i) <= buffer) && 
                    (Mathf.Abs(playerPos.y + j) <= buffer || Mathf.Abs(playerPos.y - j) <= buffer))
                {
                    HitDetector hd = Instantiate(hitDetector,
                        grid.GetWorldPosition(i, j) + new Vector3(cellSize / 2, 0, cellSize / 2),
                        this.transform.localRotation);
                    hd.x = i;
                    hd.y = j;
                }
            }
        }
    }

    public void SetGridValue(int i, int j, bool isCollided)
    {
        grid.SetValue(i, j, isCollided);
    }

    public MapGrid GetGrid()
    {
        return grid;
    }
}
