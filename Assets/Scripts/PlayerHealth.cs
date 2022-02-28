using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maximumHealth = 100f;
    public float currentHealth;
    public GameManager gm;
    public Text deathText;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        if (deathText)
        {
            deathText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            //print("Player died.");
            if (deathText)
            {
                deathText.gameObject.SetActive(true);
            }
            
            //restart level after 2 seconds
            Invoke("PlayerDied", 2);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        print("Took damage! currentHealth = " + currentHealth + ".");
    }

    public void PlayerDied()
    {

        gm.Restart();
    }
}
