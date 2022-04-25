using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public Dialogue initDialogue;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PauseGameForDialogue>().PauseForDialogue(initDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossDefeated() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
