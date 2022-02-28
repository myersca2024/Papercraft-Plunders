using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour 
{
    bool damaged = false;
    public GameObject player;
    public float damage = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCollision() && !damaged) {
            damaged = true;
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private bool CheckCollision() {
        return (Mathf.Abs(player.transform.position.x - transform.position.x) <= 3 && transform.position.z == player.transform.position.z);
    }
}
