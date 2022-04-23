using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGameForDialogue : MonoBehaviour
{
    public static bool paused = false;

    public bool loadsScene = false;

    public Image name;
    public Image text;
    public Image portrait;

    private void Update() {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && paused) {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    public void PauseForDialogue(Dialogue dialogue) {
        name.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        portrait.gameObject.SetActive(true);

        Time.timeScale = 0;
        PauseGameForDialogue.paused = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void UnpauseForDialogue() {
        name.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        portrait.gameObject.SetActive(false);
        Time.timeScale = 1;
        PauseGameForDialogue.paused = false;

        if (loadsScene) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
