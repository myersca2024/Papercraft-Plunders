using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class PlayerController : MonoBehaviour {
    public float moveDelay = .1f;

    private DeckManager dm;
    private GridObject go;
    private float timePassed;
    private Vector2 movement;
    private int activeCard = 0;

    // Start is called before the first frame update
    void Start() {
        dm = FindObjectOfType<DeckManager>();
        go = FindObjectOfType<GridObject>();
        timePassed = moveDelay;
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
    }

    // Update is called once per frame
    void Update() {
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


        dm.DiscardCard(index);
        activeCard = 0;
    }
}
