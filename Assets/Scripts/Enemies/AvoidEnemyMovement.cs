using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidEnemyMovement : MonoBehaviour {
    public float moveDelay = 1.0f;
    public float attackDelay = 5.0f;
    private float timePassed;
    private float attackTimer;
    public GameObject player;
    public float damage = 20f;
    public GameObject laser;

    private GridObject go;


    // Start is called before the first frame update
    void Start() {
        timePassed = 0;
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        go = FindObjectOfType<GridObject>();
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
    }

    // Update is called once per frame
    void Update() {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay)
        {
            this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position += this.AvoidMovement());
            timePassed = 0.0f;
        }

        if (attackTimer >= attackDelay) {
            LaserAttack();
            attackTimer = 0.0f;
            timePassed = 0;
        }

        /*
        if (CheckCollision() && attackTimer >= attackDelay) {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("we hit the player");
            attackTimer = 0.0f;
        }
        */
    }

    private Vector3 AvoidMovement() {
        Vector3 nextStep = new Vector3(0, 0, 0);

        if (player.transform.position.x == transform.position.x) {
            nextStep.x = -2 * Random.Range(0, 2) + 1;
            return nextStep;
        }

        if (player.transform.position.z > transform.position.z) {
            nextStep.z = 1 * go.cellSize;
            return nextStep;
        } else if (player.transform.position.z < transform.position.z) {
            nextStep.z = -1 * go.cellSize;
            return nextStep;
        }

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 60) {
            if (player.transform.position.x > transform.position.x) {
                nextStep.x = -1 * go.cellSize;
            } else if (player.transform.position.x < transform.position.x) {
                nextStep.x = 1 * go.cellSize;
            }
        } else if (Mathf.Abs(player.transform.position.x - transform.position.x) > 60) {
            if (player.transform.position.x > transform.position.x) {
                nextStep.x = 1 * go.cellSize;
            } else if (player.transform.position.x < transform.position.x) {
                nextStep.x = -1 * go.cellSize;
            }
        }

        return nextStep;
    }

    private bool CheckCollision() {
        return player.transform.position == transform.position;
    }

    private void LaserAttack() {
        Vector3 difference;
        if (player.transform.position.x >= transform.position.x) {
            difference = new Vector3(33, .1f, 0);
            Instantiate(laser, transform.position + difference, Quaternion.LookRotation(new Vector3(-1, 0, 0)));
        } else {
            difference = new Vector3(-33, .1f, 0);
            Instantiate(laser, transform.position + difference, Quaternion.LookRotation(new Vector3(1, 0, 0)));
        }
        
    }
}
