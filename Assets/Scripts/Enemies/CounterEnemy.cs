using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterEnemy : MonoBehaviour
{
    public float moveDelay = 1.0f;
    public float attackDelay = 5.0f;
    private float timePassed;
    private float attackTimer;
    public GameObject player;
    public float counterTime = 2.0f;

    private GridObject go;
    public bool counterStance = false;
    private Material material;
    private float timeSinceCounter = 0.0f;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = moveDelay;
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");

        List<Material> m = new List<Material>();
        gameObject.GetComponentInChildren<SpriteRenderer>().GetMaterials(m);
        material = m[0];
        color = material.color;

        go = FindObjectOfType<GridObject>();
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay && !counterStance)
        {
            this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position += this.GetNextStepToPlayer());
            timePassed = 0.0f;
        }

        //sets color to blue every frame if countering--
        //if the enemy is hit while countering then its color gets reset to normal, even if it's still countering
        if (counterStance)
        {
            material.SetColor("_Color", Color.blue);
        }

        if (attackTimer >= attackDelay && !counterStance)
        {
            Debug.Log("We are impervious to damage!!!!");
            counterStance = !counterStance;
            material.SetColor("_Color", Color.blue);
            timeSinceCounter = timePassed;
            attackTimer = 0.0f;
            Invoke("EndCounter", counterTime);
        }
        // Debug.Log("The time passed is " + timePassed + " the time since the counter started is " + timeSinceCounter);
        if (timePassed - timeSinceCounter == counterTime && counterStance) 
        {
            Debug.Log("No more countering");
            material.SetColor("_Color", color);
            counterStance = !counterStance;
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
            player.GetComponent<PlayerHealth>().TakeDamage(10.0f);
        }
        if (other.CompareTag("Hitbox") && counterStance) 
        {
            Hitbox hb;
            other.gameObject.TryGetComponent<Hitbox>(out hb);
            GetComponent<EnemyHealth>().TakeDamage(-1 * hb.damage);
            player.GetComponent<PlayerHealth>().TakeDamage(hb.damage);
        }
    }

    private void EndCounter() 
    {
        Debug.Log("No more countering");
        material.SetColor("_Color", color);
        counterStance = !counterStance;
    }
}
