using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Image creditsBox;

    public void ShowCredits()
    {
        creditsBox.gameObject.SetActive(!creditsBox.IsActive());
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
