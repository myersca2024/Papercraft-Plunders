using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;

    private MapGrid grid;

    void Start()
    {
        grid = new MapGrid(width, height, cellSize, this.transform.position);
    }

    public MapGrid GetGrid()
    {
        return grid;
    }
}
