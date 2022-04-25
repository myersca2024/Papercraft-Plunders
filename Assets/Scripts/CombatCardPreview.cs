using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCardPreview : MonoBehaviour
{
    public GameObject hitboxPreview;
    public Camera mc;

    private CombatCard currentCard;
    private List<GameObject> hitboxes;
    private GridObject go;

    private void Start()
    {
        hitboxes = new List<GameObject>();
        go = FindObjectOfType<GridObject>();
    }

    public void AttackPreview(CombatCard cc)
    {
        currentCard = cc;
        ClearPreview();
        LoadNewCard(cc);
    }

    public void LoadNewCard(CombatCard cc)
    {
        for (int i = 0; i < cc.pattern.GridSize.x; i++)
        {
            for (int j = 0; j < cc.pattern.GridSize.y; j++)
            {
                if (cc.pattern.GetCell(i, j))
                {
                    int relX = (cc.pattern.GridSize.x / 2) - i;
                    int relY = (cc.pattern.GridSize.y / 2) - j;
                    if (cc.multidirectional)
                    {
                        Vector2 screenCenter = mc.WorldToScreenPoint(this.transform.position);
                        Vector2 mousePosition = Input.mousePosition;
                        float angle = Mathf.Atan2(mousePosition.x - screenCenter.x, mousePosition.y - screenCenter.y) / Mathf.PI * 180f;
                        if (angle < 0) angle += 360f;
                        // UP
                        /*
                        if (angle > 315 || angle < 45)
                        {
                            Do nothing, default should be up
                        }
                        */
                        // RIGHT
                        if (angle > 45 && angle < 135)
                        {
                            int temp = relX;
                            relX = relY;
                            relY = -temp;
                        }
                        // DOWN
                        else if (angle > 135 && angle < 225)
                        {
                            relX = -relX;
                            relY = -relY;
                        }
                        // LEFT
                        else if (angle > 225 && angle < 315)
                        {
                            int temp = relX;
                            relX = -relY;
                            relY = temp;
                        }
                    }
                    Vector2Int playerCoords = go.GetGrid().GetXY(this.transform.position);
                    Vector2Int hitboxCoords = new Vector2Int(playerCoords.x + relX, playerCoords.y + relY);
                    Vector3 offset = new Vector3(go.cellSize / 2, 0f, go.cellSize / 2);
                    GameObject hb = Instantiate(hitboxPreview, go.GetGrid().GetWorldPosition(hitboxCoords.x, hitboxCoords.y) + offset, this.transform.localRotation);
                    hitboxes.Add(hb);
                }
            }
        }
    }

    public void ClearPreview()
    {
        foreach (GameObject hb in hitboxes)
        {
            Destroy(hb);
        }
        hitboxes.Clear();
    }
}
