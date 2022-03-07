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
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        currentHealth = DataStorage.currentHealth;
        healthSlider.value = currentHealth;
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
                PlayerController.freeze = true;
                deathText.gameObject.SetActive(true);
            }
            
            //restart level after 2 seconds
            Invoke("PlayerDied", 2);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        DataStorage.TakeDamage(damage);
        Mathf.Clamp(currentHealth, 0, 100);
        print("Took damage! currentHealth = " + currentHealth + ".");
    }

    public void PlayerDied() {
        DataStorage.currentHealth = maximumHealth;
        PlayerController.freeze = false;
        gm.Restart();
    }
}
