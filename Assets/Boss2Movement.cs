using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Movement : MonoBehaviour
{
    public float moveDelay = 1.0f;
    private float oldMoveDelay;
    public float meleeAttackDelay = 1.0f;
    private float timePassed;
    private float meleeTimer;
    public GameObject player;
    public float damage = 10f;
    public GameObject enemyProjectile;

    private GridObject go;

    private float rangedTimer;
    public float rangedAttackCooldown = 2;

    

    private int rotateIndex = 1;
    private bool fanAttacking = false;
    private float fanTimer;
    public float fanDelay = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = moveDelay;
        meleeTimer = meleeAttackDelay;
        rangedTimer = 0;
        fanTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        go = FindObjectOfType<GridObject>();
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
        oldMoveDelay = moveDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;
        fanTimer += Time.deltaTime;

        if (timePassed >= moveDelay)
        {
            this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position += this.GetNextStepToPlayer());
            timePassed = 0.0f;
        }

        if (rangedTimer >= rangedAttackCooldown)
        {
            rangedTimer = 0;
            moveDelay = oldMoveDelay;
            fanAttacking = false;

            int attack = Random.Range(0, 2);
            switch (attack)
            {
                case 0:
                    SpeedUp();
                    break;
                case 1:
                    fanAttacking = true;
                    //FanAttack();
                    break;
            }
        }

        if (fanAttacking && fanTimer >= fanDelay)
        {
            FanAttack();
            fanTimer = 0.0f;
        }

    }

    private void SpeedUp()
    {
        moveDelay /= 2;
    }

    private void FanAttack()
    {
        /*rotateIndex = (rotateIndex + 1) % 2;
        
        if (rotateIndex == 0)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 0)));
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 0)));
            Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(0, 0, 1)));
            Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(0, 0, -1)));
        } 
        if (rotateIndex == 0)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 2)));
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 2)));
            Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(1, 0, -2)));
            Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(-1, 0, -2)));
        }
        else if (rotateIndex == 1)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(2, 0, 1)));
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-2, 0, 1)));
            Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(2, 0, -1)));
            Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(-2, 0, -1)));
        }
        */
        
        Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 2)));
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 2)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(1, 0, -2)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(-1, 0, -2)));

        Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(2, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-2, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(2, 0, -1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(-2, 0, -1)));
    }

    private Vector3 GetNextStepToPlayer()
    {
        Vector3 nextStep = new Vector3(0, 0, 0);
        
        // basically RushEnemyMovement, but can move diagonally
        if (player.transform.position.x > transform.position.x)
        {
            nextStep.x = 1 * go.cellSize;
        }
        if (player.transform.position.x < transform.position.x)
        {
            nextStep.x = -1 * go.cellSize;
        }
        if (player.transform.position.z > transform.position.z)
        {
            nextStep.z = 1 * go.cellSize;
        }
        if (player.transform.position.z < transform.position.z)
        {
            nextStep.z = -1 * go.cellSize;
        }

        return nextStep;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (meleeTimer >= meleeAttackDelay)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
                meleeTimer = 0.0f;
            } 
        }
    }

}
