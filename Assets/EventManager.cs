using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private PlayerController playercontroller;
    private PlayerHealth playerhealth;
    private Image healthBarFill;
    public float eventTimer = 30.0f;
    public float elapsedTime = 0.0f;

    public float currentEventTimeLimit = 5.0f;
    public float currentEventTime = 0.0f;
    public int numEvents = 3;
    public EventState currentState;

    private float oldPlayerMoveDelay;
    private float oldAvoidMoveDelay = 1.0f;
    private float oldRandomMoveDelay = 1.0f;
    private float oldRushMoveDelay = 1.0f;
    private GameObject[] enemies;

    public enum EventState
    {
        None, 
        DoubleEnemySpeed,
        DoublePlayerSpeed,
        MakePlayerInvincible
    };



    // Start is called before the first frame update
    void Start()
    {
        
        elapsedTime = 0.0f;
        currentEventTime = 0.0f;
        playercontroller = FindObjectOfType<PlayerController>();
        playerhealth = FindObjectOfType<PlayerHealth>();
        currentState = EventState.None;
        oldPlayerMoveDelay = playercontroller.moveDelay;
        healthBarFill = GameObject.FindGameObjectWithTag("HealthBarFill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (currentState != EventState.None)
        {
            currentEventTime += Time.deltaTime;
        }
        

        if (elapsedTime >= eventTimer)
        {
            elapsedTime = 0.0f;
            int rand = Random.Range(0, numEvents);
            switch(rand) {
                case 0:
                    //nothing happens
                    currentState = EventState.None;
                    break;
                case 1:
                    //enemies double in speed for 5 seconds
                    currentState = EventState.DoubleEnemySpeed;
                    DoubleEnemySpeed();
                    break;
                case 2:
                    // player doubles in speed for 5 seconds
                    currentState = EventState.DoublePlayerSpeed;
                    DoublePlayerSpeed();
                    break;
                case 3:
                    //player becomes invincible for 5 seconds
                    currentState = EventState.MakePlayerInvincible;
                    healthBarFill.color = new Color(0, 0, 255);
                    break;
                default:
                    Debug.Log("defaulted for some reason, should not be here");
                    break;
            }
        }


        switch (currentState)
        {
            case EventState.None:
                break;
            case EventState.DoubleEnemySpeed:
                //only want to call this once?
                break;
            case EventState.DoublePlayerSpeed:
               //only want to call this once?
                break;
            case EventState.MakePlayerInvincible:
                MakePlayerInvincible();
                break;
            default:
                Debug.Log("defaulted for some reason, should not be here");
                break;
        }

        if (currentEventTime >= currentEventTimeLimit)
        {
            elapsedTime = 0.0f; // comment out if the event timer should reset after an event, uncomment if events can happen very soon after each other
            currentEventTime = 0.0f;
            currentState = EventState.None;
            Debug.Log("event is over");
            healthBarFill.color = new Color(255, 0, 0);
            if (playercontroller.moveDelay != oldPlayerMoveDelay)
            {
                playercontroller.moveDelay = oldPlayerMoveDelay;
            }
            if (enemies != null)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy != null)
                    {
                        if (enemy.GetComponent<AvoidEnemyMovement>() != null)
                        {
                            enemy.GetComponent<AvoidEnemyMovement>().moveDelay = oldAvoidMoveDelay;
                        }
                        else if (enemy.GetComponent<RandomEnemyMovement>() != null)
                        {
                            enemy.GetComponent<RandomEnemyMovement>().moveDelay = oldRandomMoveDelay;
                        }
                        else if (enemy.GetComponent<RushEnemyMovement>() != null)
                        {
                            enemy.GetComponent<RushEnemyMovement>().moveDelay = oldRushMoveDelay;
                        }
                    }
                }
            }
            
        }
    }

    private void MakePlayerInvincible()
    {
        Debug.Log("making player invincible!");
        //just setting the player's current health to max health constantly
        playerhealth.currentHealth = playerhealth.maximumHealth;

    }

    private void DoublePlayerSpeed()
    {
        Debug.Log("doubling player speed!");
        playercontroller.moveDelay /= 2;
    }

    private void DoubleEnemySpeed()
    {
        Debug.Log("doubling enemy speed!");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<AvoidEnemyMovement>() != null)
                {
                    enemy.GetComponent<AvoidEnemyMovement>().moveDelay /= 2;
                }
                else if (enemy.GetComponent<RandomEnemyMovement>() != null)
                {
                    enemy.GetComponent<RandomEnemyMovement>().moveDelay /= 2;
                }
                else if (enemy.GetComponent<RushEnemyMovement>() != null)
                {
                    enemy.GetComponent<RushEnemyMovement>().moveDelay /= 2;
                }
            }
        }
    }
}
