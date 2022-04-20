using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Image creditsBox;

    public AudioClip creditsSFX;
    public AudioClip startSFX;

    public void ShowCredits()
    {
        AudioSource.PlayClipAtPoint(creditsSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
        creditsBox.gameObject.SetActive(!creditsBox.IsActive());
    }

    public void StartGame() {
        AudioSource.PlayClipAtPoint(creditsSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);

        Invoke("NextScene", 1);
    }

    private void NextScene() {
        SceneManager.LoadScene(1);
    }
}
