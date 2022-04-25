using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;
    public GameObject deckViewerPanels;
    public AudioClip clickSFX;
    public bool isPaused = false;

    private void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0.0f;
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1.0f;
                isPaused = false;
            }
            resumeButton.gameObject.SetActive(!resumeButton.IsActive());
            quitButton.gameObject.SetActive(!quitButton.IsActive());
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        resumeButton.gameObject.SetActive(!resumeButton.IsActive());
        quitButton.gameObject.SetActive(!quitButton.IsActive());
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
    }

    public void QuitToMenu()
    {
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);

        Invoke("Quit", 1);
    }

    private void Quit()
    {
        SceneManager.LoadScene(0); //main menu should be build index 0
    }
}
