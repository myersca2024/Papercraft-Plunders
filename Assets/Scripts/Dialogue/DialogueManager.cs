using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image portrait;

    public AudioClip dialogueSFX;

    private Queue<string> names;
    private Queue<string> sentences;
    private Queue<Sprite> sprites;

    private bool scrolling = false;
    private string currentSentence = "";

    // Start is called before the first frame update
    private void Awake()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();
        sprites = new Queue<Sprite>();
    }

    public void StartDialogue(Dialogue dialogue) {
        names.Clear();
        sentences.Clear();
        sprites.Clear();

        foreach (string name in dialogue.names) {
            names.Enqueue(name);
        }
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        foreach (Sprite sprite in dialogue.sprites) {
            sprites.Enqueue(sprite);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (scrolling) {
            scrolling = false;
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            GetComponent<AudioSource>().Stop();
            return;
        }

        if (sentences.Count == 0) {
            EndDialogue();
            FindObjectOfType<PauseGameForDialogue>().UnpauseForDialogue();
            
            return;
        }

        GetComponent<AudioSource>().time = Random.Range(0.0f, 4.0f);
        GetComponent<AudioSource>().Play();

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        portrait.sprite = sprites.Dequeue();
        if (portrait.sprite == null) {
            portrait.gameObject.transform.parent.gameObject.SetActive(false);
        } else {
            portrait.gameObject.transform.parent.gameObject.SetActive(true);
        }
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
            yield return new WaitForSecondsRealtime(0.02f);
        }

        GetComponent<AudioSource>().Stop();
        scrolling = false;
    }

    void EndDialogue() {
        GetComponent<AudioSource>().Stop();
        Debug.Log("this is where u load the main game");
    }
}
