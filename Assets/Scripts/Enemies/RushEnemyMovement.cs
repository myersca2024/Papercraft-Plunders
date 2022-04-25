using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemyMovement : MonoBehaviour
{
    public float moveDelay = 1.0f;
    public float attackDelay = 1.0f;
    private float timePassed;
    private float attackTimer;
    public GameObject player;
    public float damage = 20f;
    public bool strapped = false;
    public GameObject enemyProjectile;

    private GridObject go;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = moveDelay;
        attackTimer = attackDelay;
        player = GameObject.FindGameObjectWithTag("Player");
        go = FindObjectOfType<GridObject>();
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay)
        {
            this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position += this.GetNextStepToPlayer());
            timePassed = 0.0f;
        }

        if (strapped && attackTimer >= attackDelay)
        {
            attackTimer = 0.0f;
            Shoot();
        }

        if (CheckCollision() && attackTimer >= attackDelay)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            attackTimer = 0.0f;
        }
    }

    private Vector3 GetNextStepToPlayer()
    {
        Vector3 nextStep = new Vector3(0, 0, 0);

        //Enemy will always move right/left first, then up/down (more complicated pathfinding is annoying but necessary later probably)
        if (player.transform.position.x > transform.position.x)
        {
            nextStep.x = 1 * go.cellSize;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            nextStep.x = -1 * go.cellSize;
        }
        else if (player.transform.position.z > transform.position.z)
        {
            nextStep.z = 1 * go.cellSize;
        }
        else if (player.transform.position.z < transform.position.z)
        {
            nextStep.z = -1 * go.cellSize;
        }

        return nextStep;
    }

    private bool CheckCollision()
    {
        Vector3 difference = player.transform.position - transform.position;
        return difference.magnitude <= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (attackTimer >= attackDelay)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
                attackTimer = 0.0f;
            }
        }
    }

    private void Shoot()
    {
        if (player.transform.position.x > transform.position.x)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 0)));
        }
        else if (player.transform.position.x < transform.position.x)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 0)));
        }
        else if (player.transform.position.z > transform.position.z)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(0, 0, 1)));
        }
        else if (player.transform.position.z < transform.position.z)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(0, 0, -1)));
        }
        
    }
}
