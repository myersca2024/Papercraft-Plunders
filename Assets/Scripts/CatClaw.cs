using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatClaw : MonoBehaviour
{
    public float damage = 20f;
    public float destroyDelay = 5.0f;
    public float speed = 1.0f;
    public float moveDelay = 0.1f;
    private float moveTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveDelay)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
            moveTimer = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerHealth>() != null)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyHealth>() != null)
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
}
