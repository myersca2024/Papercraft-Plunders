using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maximumHealth = 100f;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            print("Player died.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        print("Took damage! currentHealth = " + currentHealth + ".");
    }
}
