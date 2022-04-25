using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Dialogue[] dialogues;

    public float maximumHealth = 100f;
    public float currentHealth;
    public GameManager gm;
    public Text deathText;
    public Slider healthSlider;

    bool died = false;
    private bool lifeSaved = false;

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
        if (currentHealth <= 0 && !died)
        {
            
            if (!lifeSaved)
            {
                int lifeSaveRoll = Random.Range(0, 8); // 1 in 8 chance to life save
                if (lifeSaveRoll == 7)
                {
                    FindObjectOfType<EventManager>().LifeSave();
                    currentHealth = maximumHealth;
                    healthSlider.value = currentHealth;
                    
                }
                lifeSaved = true;
            }
            else
            {
                died = true;

                int rand = Random.Range(0, dialogues.Length);
                FindObjectOfType<PauseGameForDialogue>().PauseForDialogue(dialogues[rand]);

                PlayerController.freeze = true;

                //print("Player died.");
                //if (deathText)
                //{

                //    deathText.gameObject.SetActive(true);
                //}

                //restart level after 2 seconds
                Invoke("PlayerDied", 1);
            } 
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        DataStorage.TakeDamage(damage);
        Mathf.Clamp(currentHealth, 0, 100);
        //print("Took damage! currentHealth = " + currentHealth + ".");
    }

    public void PlayerDied() {
        DataStorage.currentHealth = maximumHealth;
        PlayerController.freeze = false;
        gm.Restart();
    }
}
