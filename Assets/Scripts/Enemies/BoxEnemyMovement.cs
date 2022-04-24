using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEnemyMovement : MonoBehaviour
{
    public float moveDelay = 0.5f;
    public float attackDelay = 1.0f;
    private float timePassed;
    private float attackTimer;
    public GameObject player;
    public float damage = 20f;
    public GameObject enemyProjectile;
    public int maxMoves = 4;

    private int numMovesInCurrentDirection = 0;
    private int currentDirection = 0;
    private bool rotateAttack = false;
    private Vector3 lastPosition;

    private GridObject go;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = moveDelay;
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        go = FindObjectOfType<GridObject>();
        this.transform.position = go.GetGrid().AttemptMove(this.transform.position, this.transform.position);
        lastPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay)
        {
            this.transform.position = go.GetGrid().AttemptMove(this.transform.position, transform.position += GetNextStep());
            //if we hit a wall, change direction immediately
            if (lastPosition == this.transform.position)
            {
                currentDirection = (currentDirection + 1) % 4;
                numMovesInCurrentDirection = 0;
            }
            timePassed = 0.0f;
        }

        if (attackTimer >= attackDelay)
        {
            if (rotateAttack)
            {
                RotateQuadAttack();
            }
            else
            {
                QuadAttack();
            }
            rotateAttack = !rotateAttack;
            attackTimer = 0.0f;
        }

        if (CheckCollision())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("we hit the player");
        }
    }

    private Vector3 GetNextStep()
    {
        Vector3 nextStep = new Vector3(0, 0, 0);

        //idea here is move (up to maxMoves num of times, in this case 4) 4 times, then change direction, move 4 times, repeat.
        if (numMovesInCurrentDirection >= maxMoves)
        {
            currentDirection = (currentDirection + 1) % 4;
            numMovesInCurrentDirection = 0;
        }

        if (currentDirection == 1)
        {
            nextStep.x = 1 * go.cellSize;
        }
        else if (currentDirection == 2)
        {
            nextStep.z = 1 * go.cellSize;
        }
        else if (currentDirection == 3)
        {
            nextStep.x = -1 * go.cellSize;
        }
        else if (currentDirection == 0)
        {
            nextStep.z = -1 * go.cellSize;
        }

        numMovesInCurrentDirection++;

        return nextStep;
    }

    private bool CheckCollision()
    {
        return player.transform.position == transform.position;
    }

    private void QuadAttack()
    {
        Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 0)));
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 0)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(0, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(0, 0, -1)));
    }

    private void RotateQuadAttack()
    {
        Instantiate(enemyProjectile, transform.position + new Vector3(1, 0, 0), Quaternion.LookRotation(new Vector3(1, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.LookRotation(new Vector3(-1, 0, 1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, 1), Quaternion.LookRotation(new Vector3(1, 0, -1)));
        Instantiate(enemyProjectile, transform.position + new Vector3(0, 0, -1), Quaternion.LookRotation(new Vector3(-1, 0, -1)));
    }
}
