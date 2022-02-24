using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveDelay = 1.0f;
    public float attackDelay = 1.0f;
    private float timePassed;
    private float attackTimer;
    private Vector2 movement;
    public GameObject player;
    public float damage = 20f;


    // Start is called before the first frame update
    void Start()
    {
        timePassed = moveDelay;
        attackTimer = attackDelay;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay)
        {
            transform.position += this.GetNextStepToPlayer();
            timePassed = 0.0f;
            print(timePassed);
        }
        
        if (CheckCollision() && attackTimer >= attackDelay)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("we hit the player");
            attackTimer = 0.0f;
        }
        
        
        
    }

    private Vector3 GetNextStepToPlayer()
    {
        Vector3 nextStep = new Vector3(0, 0, 0);

        //Enemy will always move right/left first, then up/down (more complicated pathfinding is annoying but necessary later probably)
        if (player.transform.position.x > transform.position.x)
        {
            nextStep.x = 1;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            nextStep.x = -1;
        }
        else if (player.transform.position.z > transform.position.z)
        {
            nextStep.z = 1;
        }
        else if (player.transform.position.z < transform.position.z)
        {
            nextStep.z = -1;
        }

        return nextStep;
    }

    private bool CheckCollision()
    {
        return player.transform.position == transform.position;
    }


}
