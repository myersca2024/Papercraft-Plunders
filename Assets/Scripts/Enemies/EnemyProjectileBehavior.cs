using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehavior : MonoBehaviour
{
    public float moveDelay = 1.0f;
    private float timePassed;
    private float attackTimer;
    public GameObject player;
    public float damage = 20f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
        velocity = transform.forward;
        timePassed = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (timePassed >= moveDelay) {
            transform.position += velocity * 10;
            timePassed = 0.0f;
        }

        if (CheckCollision()) {
            Debug.Log("This shit fuckin hit boys!!!!!!! ");
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private bool CheckCollision() {
        return player.transform.position == transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
