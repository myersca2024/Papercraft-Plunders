using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class PlayerController : MonoBehaviour {
    public float moveDelay = .1f;
    public Hitbox hitbox;

    private DeckManager dm;
    private GridObject go;
    private Camera mc;
    private float timePassed;
    private Vector2 movement;
    private int activeCard = 0;

    // Start is called before the first frame update
    void Start() {
        dm = FindObjectOfType<DeckManager>();
        go = FindObjectOfType<GridObject>();
        GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent<Camera>(out mc);
        timePassed = moveDelay;
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        // Movement
        if (timePassed >= moveDelay) {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0 || movement.y != 0) {
                timePassed = 0;
            }

            if (movement.x > 0) {
                this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position + new Vector3(1, 0));
            } else if (movement.x < 0) {
                this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position - new Vector3(1, 0));
            } else if (movement.y > 0) {
                this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position + new Vector3(0, 0, 1));
            } else if (movement.y < 0) {
                this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position - new Vector3(0, 0, 1));
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { activeCard = 0; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { activeCard = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { activeCard = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { activeCard = 3; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { activeCard = 4; }

        if (Input.GetMouseButtonDown(0))
        {
            if (dm.handSize > 0 && activeCard < dm.handSize)
            {
                UseCard(activeCard);
            }
        }
    }

    public void UseCard(int index)
    {
        CombatCard cc = dm.GetHandCard(index);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (cc.effect.pattern.GetCell(i, j))
                {
                    int relX = 2 - i;
                    int relY = 2 - j;
                    if (cc.effect.multidirectional)
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
                    Vector3 offset = new Vector3(go.cellSize / 2, 0, go.cellSize / 2);
                    Hitbox hb = Instantiate(hitbox, go.GetGrid().GetWorldPosition(hitboxCoords.x, hitboxCoords.y) + offset, this.transform.localRotation);
                    if (cc.effect.visualEffect != null)
                    {
                        Instantiate(cc.effect.visualEffect, go.GetGrid().GetWorldPosition(hitboxCoords.x, hitboxCoords.y) + offset, this.transform.localRotation);
                    }
                    hb.damage = cc.effect.damage;
                    hb.duration = cc.effect.duration;
                }
            }
        }

        dm.DiscardCard(index);
        activeCard = 0;
    }
}
