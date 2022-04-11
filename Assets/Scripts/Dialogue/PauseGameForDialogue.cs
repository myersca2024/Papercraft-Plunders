using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameForDialogue : MonoBehaviour
{
    public static bool paused = false;

    public Text nameText;
    public Text dialogueText;

    private void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    public void PauseForDialogue(Dialogue dialogue) {
        nameText.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        Time.timeScale = 0;
        PauseGameForDialogue.paused = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void UnpauseForDialogue() {
        nameText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        Time.timeScale = 1;
        PauseGameForDialogue.paused = false;
    }
}
