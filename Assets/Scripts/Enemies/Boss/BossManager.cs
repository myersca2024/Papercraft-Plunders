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
        FindObjectOfType<AudioSource>().Stop();
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < sources.Length; i++) {
            sources[i].Stop();
        }
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossDefeated() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
