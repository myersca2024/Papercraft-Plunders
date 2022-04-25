using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public Dialogue tutorial;
    public Dialogue[] dialogues;

    private PlayerController playercontroller;
    private PlayerHealth playerhealth;
    private GameObject player;
    private Image healthBarFill;
    public Sprite empoweredHealthBarFill;
    private Sprite originalHealthBarFillSprite;
    public GameObject catClaw;
    public float catClawZOffset = 175f;
    private Camera mainCamera;
    public float eventTimer = 30.0f;
    public float elapsedTime = 0.0f;
    
    public float currentEventTimeLimit = 5.0f;
    public float currentEventTime = 0.0f;
    public int numEvents = 7;
    public EventState currentState;

    private float oldPlayerMoveDelay;
    private float oldAvoidMoveDelay = 1.0f;
    private float oldRandomMoveDelay = 1.0f;
    private float oldRushMoveDelay = 1.0f;
    private float oldBoxMoveDelay = 0.5f;
    private float oldSplitMoveDelay = 1.0f;
    private float oldBoss2MoveDelay = 1.0f;
    private GameObject[] enemies;

    private GameObject playerEmpowered;
    private bool sizeIncreased = false;

    public enum EventState
    {
        None, 
        DoubleEnemySpeed,
        DoublePlayerSpeed,
        MakePlayerInvincible,
        InfiniteCCUses,
        CatClaw,
        BigPlayer
    };



    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        currentEventTime = 0.0f;
        playercontroller = FindObjectOfType<PlayerController>();
        playerhealth = FindObjectOfType<PlayerHealth>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = EventState.None;
        oldPlayerMoveDelay = playercontroller.moveDelay;
        healthBarFill = GameObject.FindGameObjectWithTag("HealthBarFill").GetComponent<Image>();
        originalHealthBarFillSprite = healthBarFill.sprite;
        playerEmpowered = GameObject.FindGameObjectWithTag("PlayerEmpowered");
        playerEmpowered.SetActive(false);
        mainCamera = Camera.main;

        if (!DataStorage.startTutorial) {
            FindObjectOfType<PauseGameForDialogue>().PauseForDialogue(tutorial);
            DataStorage.startTutorial = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (currentState != EventState.None)
        {
            currentEventTime += Time.deltaTime;
        }
        

        if (elapsedTime >= eventTimer && Time.timeScale != 0)
        {
            elapsedTime = 0.0f;
            int rand = Random.Range(0, numEvents + 1);
            switch(rand) {
                case 0:
                    //nothing happens
                    currentState = EventState.None;
                    break;
                case 1:
                    FirstDialogue(rand - 1);

                    //enemies double in speed for 5 seconds
                    currentState = EventState.DoubleEnemySpeed;
                    DoubleEnemySpeed();
                    break;
                case 2:
                    FirstDialogue(rand - 1);

                    // player doubles in speed for 5 seconds
                    currentState = EventState.DoublePlayerSpeed;
                    DoublePlayerSpeed();
                    break;
                case 3:
                    FirstDialogue(rand - 1);

                    //player becomes invincible for 5 seconds
                    currentState = EventState.MakePlayerInvincible;
                    healthBarFill.sprite = empoweredHealthBarFill;
                    playerEmpowered.SetActive(true);
                    //healthBarFill.color = new Color(0, 0, 255);
                    break;
                case 4:
                    FirstDialogue(rand - 1);

                    currentState = EventState.InfiniteCCUses; 
                    playerEmpowered.SetActive(true);
                    playerEmpowered.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
                    InfiniteCombatCardUses();
                    break;
                case 5:
                    FirstDialogue(rand - 1);

                    currentState = EventState.CatClaw;
                    CatClaw();
                    break;
                case 6:
                    FirstDialogue(rand - 1);

                    currentState = EventState.BigPlayer;
                    BigPlayer();
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
            case EventState.InfiniteCCUses:
                 // could call this multiple times, doesnt really matter but redundant method calls
                break;
            case EventState.CatClaw:
                // only make 1 cat claw lol
                break;
            case EventState.BigPlayer:
                // probably dont want to constantly increase in size
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
            //Debug.Log("event is over");
            //healthBarFill.color = new Color(255, 0, 0);
            healthBarFill.sprite = originalHealthBarFillSprite;
            playerEmpowered.SetActive(false);
            playerEmpowered.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
            playercontroller.infiniteUses = false;
            if (sizeIncreased)
            {
                player.transform.localScale -= new Vector3(1.5f, 1, 1.5f);
                sizeIncreased = false;
            }
            
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
                        else if (enemy.GetComponent<BoxEnemyMovement>() != null)
                        {
                            enemy.GetComponent<BoxEnemyMovement>().moveDelay = oldBoxMoveDelay;
                        }
                        else if (enemy.GetComponent<SplitEnemy>() != null)
                        {
                            enemy.GetComponent<SplitEnemy>().moveDelay = oldSplitMoveDelay;
                        }
                        else if (enemy.GetComponent<Boss2Movement>() != null)
                        {
                            enemy.GetComponent<Boss2Movement>().moveDelay = oldBoss2MoveDelay;
                        }
                    }
                }
            }
            
        }
    }

    private void FirstDialogue(int i) {
        FindObjectOfType<PauseGameForDialogue>().PauseForDialogue(dialogues[i]);
    }

    private void MakePlayerInvincible()
    {
        //Debug.Log("making player invincible!");
        //just setting the player's current health to max health constantly
        playerhealth.currentHealth = playerhealth.maximumHealth;

    }

    private void DoublePlayerSpeed()
    {
        //Debug.Log("doubling player speed!");
        playercontroller.moveDelay /= 2;
    }

    private void InfiniteCombatCardUses()
    {
        //flip a variable in the player controller that causes the cc.decrementuses() function to not be called (in use combat card function, line 173)
        playercontroller.infiniteUses = true;
    }

    private void CatClaw()
    {
        //cat claw moves across the screen (vertically-- hits everything but dodgeable, horizontal would be impossible to dodge but hit only enemies?)
        float xPos = Random.Range(-110, 110);
        
        Vector3 clawPosition = new Vector3(player.transform.position.x + xPos, 0, player.transform.position.z + catClawZOffset);
        Instantiate(catClaw, clawPosition, transform.rotation);
    }

    private void BigPlayer()
    {
        player.transform.localScale += new Vector3(1.5f, 1, 1.5f);
        sizeIncreased = true;
    }

    private void DoubleEnemySpeed()
    {
        //Debug.Log("doubling enemy speed!");
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
                else if (enemy.GetComponent<BoxEnemyMovement>() != null)
                {
                    enemy.GetComponent<BoxEnemyMovement>().moveDelay /= 2;
                }
                else if (enemy.GetComponent<SplitEnemy>() != null)
                {
                    enemy.GetComponent<SplitEnemy>().moveDelay /= 2;
                }
                else if (enemy.GetComponent<Boss2Movement>() != null)
                {
                    enemy.GetComponent<Boss2Movement>().moveDelay /= 2;
                }
            }
        }
    }
}
