using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public HitDetector hitDetector;

    public float lineThickness;
    public Color lineColor;
    public Shader lineShader;

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
        DrawGrid();
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

    public void DrawGrid()
    {
        Vector3 buffer = new Vector3(0, 0.1f, 0);
        for (int i = 0; i < width; i++)
        {
            DrawLine(grid.GetWorldPosition(i, 0) + buffer, grid.GetWorldPosition(i, height) + buffer, Color.red);
        }
        for (int i = 0; i < width; i++)
        {
            DrawLine(grid.GetWorldPosition(0, i) + buffer, grid.GetWorldPosition(width, i) + buffer, Color.red);
        }
        /*
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                DrawLine(grid.GetWorldPosition(i, j), grid.GetWorldPosition(i, j + 1), Color.red);
                DrawLine(grid.GetWorldPosition(i, j), grid.GetWorldPosition(i + 1, j), Color.red);
            }
        }
        DrawLine(grid.GetWorldPosition(0, height), grid.GetWorldPosition(width, height), Color.red);
        DrawLine(grid.GetWorldPosition(width, 0), grid.GetWorldPosition(width, height), Color.red);
        */
    }

    // https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(lineShader);
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = lineThickness;
        lr.endWidth = lineThickness;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        // GameObject.Destroy(myLine, duration);
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
