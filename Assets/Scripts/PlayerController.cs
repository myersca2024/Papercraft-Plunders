using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class PlayerController : MonoBehaviour {
    public float moveDelay = .1f;
    public float attackBuffer = .3f;
    public Hitbox hitbox;
    public static bool freeze = false;

    private DeckManager dm;
    private PlayerHealth playerHealth;
    private GridObject go;
    private CombatCardPreview preview;
    private Camera mc;
    private float timePassed;
    private Vector2 movement;
    private int activeCard = 0;
    private float attackTimer;

    // Start is called before the first frame update
    void Start() {
        dm = FindObjectOfType<DeckManager>();
        go = FindObjectOfType<GridObject>();
        playerHealth = GetComponent<PlayerHealth>();
        preview = GetComponent<CombatCardPreview>();
        GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent<Camera>(out mc);
        timePassed = moveDelay;
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
        attackTimer = attackBuffer;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (!freeze)
        {
            // Movement
            if (timePassed >= moveDelay && attackTimer >= attackBuffer)
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                if (movement.x != 0 || movement.y != 0)
                {
                    timePassed = 0;
                }

                if (movement.x > 0)
                {
                    this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position + new Vector3(go.cellSize, 0));
                }
                else if (movement.x < 0)
                {
                    this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position - new Vector3(go.cellSize, 0));
                }
                else if (movement.y > 0)
                {
                    this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position + new Vector3(0, 0, go.cellSize));
                }
                else if (movement.y < 0)
                {
                    this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position - new Vector3(0, 0, go.cellSize));
                }
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                activeCard++;
                if (activeCard >= dm.handSize)
                {
                    activeCard = 0;
                }
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                activeCard--;
                if (activeCard < 0)
                {
                    activeCard = dm.handSize - 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) { activeCard = 0; }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { activeCard = 1; }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { activeCard = 2; }

            if (activeCard < dm.handSize)
            {
                dm.HighlightCard(activeCard);
                if (dm.handSize != 0) preview.AttackPreview(dm.GetHandCard(activeCard));

                if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
                {
                    if (dm.handSize > 0 && activeCard < dm.handSize && attackTimer >= attackBuffer)
                    {
                        UseCombatCard(this.transform.position, activeCard);
                    }
                }
            }
        }
        else
        {
            preview.ClearPreview();
        }
    }

    public void UseCombatCard(Vector3 center, int index)
    {
        attackTimer = 0;
        CombatCard cc = dm.GetHandCard(index);
        playerHealth.TakeDamage(-cc.healing);
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
                    Vector2Int playerCoords = go.GetGrid().GetXY(center);
                    Vector2Int hitboxCoords = new Vector2Int(playerCoords.x + relX, playerCoords.y + relY);
                    Vector3 offset = new Vector3(go.cellSize / 2, 0, go.cellSize / 2);
                    Hitbox hb = Instantiate(hitbox, go.GetGrid().GetWorldPosition(hitboxCoords.x, hitboxCoords.y) + offset, this.transform.localRotation);
                    if (cc.visualEffect != null)
                    {
                        Instantiate(cc.visualEffect, go.GetGrid().GetWorldPosition(hitboxCoords.x, hitboxCoords.y) + offset, this.transform.localRotation);
                    }
                    hb.damage = cc.damage;
                    hb.duration = cc.duration;
                }
            }
        }

        cc.DecrementUses();
        if (cc.GetUses() == 0)
        {
            dm.DiscardCard(index);
            cc.RefreshUses();

            if (activeCard >= dm.handSize + dm.deckSize) {
                activeCard = dm.handSize - 1;
            }
        }
    }
}
