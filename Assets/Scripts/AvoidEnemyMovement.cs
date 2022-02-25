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


    // Start is called before the first frame update
    void Start() {
        timePassed = moveDelay;
        attackTimer = attackDelay;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay) {
            transform.position += this.AvoidMovement();
            timePassed = 0.0f;
        }

        if (CheckCollision() && attackTimer >= attackDelay) {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("we hit the player");
            attackTimer = 0.0f;
        }
    }

    private Vector3 AvoidMovement() {
        Vector3 nextStep = new Vector3(0, 0, 0);

        if (player.transform.position.x == transform.position.x) {
            nextStep.x = -2 * Random.Range(0, 2) + 1;
            return nextStep;
        } else if (player.transform.position.z == transform.position.z) {
            nextStep.z = -2 * Random.Range(0, 2) + 1;
            return nextStep;
        }

        if (player.transform.position.x > transform.position.x) {
            nextStep.x = -1;
        } else if (player.transform.position.x < transform.position.x) {
            nextStep.x = 1;
        } else if (player.transform.position.z > transform.position.z) {
            nextStep.z = -1;
        } else if (player.transform.position.z < transform.position.z) {
            nextStep.z = 1;
        }

        return nextStep;
    }

    private bool CheckCollision() {
        return player.transform.position == transform.position;
    }


}
