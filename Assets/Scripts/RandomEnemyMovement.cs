using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyMovement : MonoBehaviour {
    public float moveDelay = 1.0f;
    public float attackDelay = 5.0f;
    private float timePassed;
    private float attackTimer;
    public GameObject player;
    public float damage = 20f;
    public GameObject enemyProjectile;


    // Start is called before the first frame update
    void Start() {
        timePassed = moveDelay;
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay) {
            transform.position += this.RandomMovement();
            timePassed = 0.0f;
        }

        if (attackTimer >= attackDelay) {
            QuadAttack();
            attackTimer = 0.0f;
        }

        if (CheckCollision()) {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("we hit the player");
        }
    }

    private Vector3 RandomMovement() {
        Vector3 nextStep = new Vector3(0, 0, 0);

        int axis = Random.Range(0, 2);
        int dir = Random.Range(0, 2);

        if (dir == 0) {
            dir = -1;
        }
        if (axis == 0) {
            nextStep.x += dir;
        } else {
            nextStep.z += dir;
        }

        return nextStep;
    }

    private bool CheckCollision() {
        return player.transform.position == transform.position;
    }

    private void QuadAttack() {
        Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 0)));
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 0)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(0, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(0, 0, -1)));
    }
}