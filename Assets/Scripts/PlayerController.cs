using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveDelay = .1f;
    private float timePassed;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start() {
        timePassed = moveDelay;
    }

    // Update is called once per frame
    void Update() {
        timePassed += Time.deltaTime;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (timePassed >= moveDelay) {
            timePassed = 0;
            print("time passed move delay");

            if (movement.x > 0) {
                transform.position = transform.position + new Vector3(1, 0);
            } else if (movement.x < 0) {
                transform.position = transform.position - new Vector3(1, 0);
            } else if (movement.y > 0) {
                transform.position = transform.position + new Vector3(0, 0, 1);
            } else {
                transform.position = transform.position - new Vector3(0, 0, 1);
            }
        }
    }
}
