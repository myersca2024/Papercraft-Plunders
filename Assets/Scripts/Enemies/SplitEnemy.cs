using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : MonoBehaviour
{
    public float moveDelay = 1.0f;
    public float attackDelay = 5.0f;
    public GameObject player;
    public float damage = 20f;
    public GameObject splitEnemy;
    public Vector3 offset;

    private float timePassed;
    private float attackTimer;
    private GridObject go;
    private EnemyHealth enemyHealth;

    void Start()
    {
        timePassed = moveDelay;
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        go = FindObjectOfType<GridObject>();
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);

        offset.x = offset.x * go.cellSize;
        offset.z = offset.z * go.cellSize;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay)
        {
            this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position += this.RandomMovement());
            timePassed = 0.0f;
        }

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0.0f;
        }

        if (CheckCollision())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("we hit the player");
        }

        if (enemyHealth.currentHealth <= 0) 
        {
            SplitIntoTwo();
        }
    }
    private Vector3 RandomMovement()
    {
        Vector3 nextStep = new Vector3(0, 0, 0);

        int axis = Random.Range(0, 2);
        int dir = Random.Range(0, 2);

        if (dir == 0)
        {
            dir = -1;
        }
        if (axis == 0)
        {
            nextStep.x += dir * go.cellSize;
        }
        else
        {
            nextStep.z += dir * go.cellSize;
        }

        return nextStep;
    }

    private bool CheckCollision()
    {
        return player.transform.position == transform.position;
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

    void SplitIntoTwo() 
    {
        if (splitEnemy != null) 
        {
            Debug.Log("Splitting into more enemies"); 
            Instantiate(splitEnemy, transform.position, transform.rotation);
            Instantiate(splitEnemy, transform.position, transform.rotation);
            Debug.Log("More enemies have been made");
        }
    }
}
