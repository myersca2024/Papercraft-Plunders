using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBall : MonoBehaviour
{
    private float attackTimer;
    public GameObject player;
    public float attackCooldown = 5;

    public GameObject laser;
    public GameObject enemyProjectile;
    public GameObject bigBossProjectile;


    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown) 
        {
            attackTimer = 0;

            int attack = Random.Range(0, 3);
            switch (attack) {
                case 0:
                    SmallFireBall();
                    break;
                case 1:
                    BigFireBall();
                    break;
                case 2:
                    BeamAttack();
                    break;
            }
        }
    }

    void BigFireBall() 
    {
        if (player.transform.position.z > transform.position.z)
        {
            Instantiate(bigBossProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(0, 0, 1)));
        }
        else 
        {
            Instantiate(bigBossProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(0, 0, -1)));
        }
    }

    void BeamAttack() 
    {
        Vector3 difference;
        if (player.transform.position.x >= transform.position.x)
        {
            difference = new Vector3(3, 0, 0);
        }
        else
        {
            difference = new Vector3(-3, 0, 0);
        }
        Instantiate(laser, transform.position + difference, Quaternion.LookRotation(new Vector3(1, 500, 0)));
    }

    void SmallFireBall() 
    {
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 0)));
        Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 0)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(0, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(0, 0, -1)));
    }
}
