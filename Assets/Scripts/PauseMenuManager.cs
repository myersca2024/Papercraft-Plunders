using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject CCMenu;
    public GameObject RCMenu;
    public AudioClip clickSFX;
    public bool isPaused = false;
    public bool inMenu = false;

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
            if (inMenu)
            {
                inMenu = false;
            }
            else
            {
                pausePanel.SetActive(!pausePanel.activeSelf);
            }
            CCMenu.SetActive(false);
            RCMenu.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        pausePanel.SetActive(!pausePanel.activeSelf);
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
    }

    public void ViewCombatCards()
    {
        CCMenu.SetActive(true);
        pausePanel.SetActive(!pausePanel.activeSelf);
        inMenu = true;
}

    public void ViewDungeonCards()
    {
        RCMenu.SetActive(true);
        pausePanel.SetActive(!pausePanel.activeSelf);
        inMenu = true;
    }

    public void SwitchInMenu()
    {
        inMenu = !inMenu;
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
