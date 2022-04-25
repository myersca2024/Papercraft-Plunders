using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static float currentHealth = 200;

    public static bool startTutorial = false;
    public static bool chestTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void TakeDamage(float damage) {
        currentHealth -= damage;
    }
}
