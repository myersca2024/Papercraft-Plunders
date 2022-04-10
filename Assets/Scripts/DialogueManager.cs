using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogue;

    public Text nameText;
    public Text dialogueText;

    private Queue<string> names;
    private Queue<string> sentences;

    private bool scrolling = false;
    private string currentSentence = "";

    // Start is called before the first frame update
    void Start()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();

        names.Clear();
        sentences.Clear();

        foreach (string name in dialogue.names) {
            names.Enqueue(name);
        }
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        if (scrolling) {
            scrolling = false;
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            return;
        }

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        currentSentence = sentence;

        nameText.text = name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        scrolling = true;

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        scrolling = false;
    }

    void EndDialogue() {
        Debug.Log("this is where u load the main game");
    }
}
