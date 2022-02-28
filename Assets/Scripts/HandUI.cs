using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandUI : MonoBehaviour
{
    public int id;
    public DeckManager dm;
    public TMP_Text cardName;
    public Image cardImage;

    private void Start()
    {
        dm = FindObjectOfType<DeckManager>();
    }

    private void Update()
    {
        if (id + 1 > dm.handSize)
        {
            Disable();
        }
        else
        {
            Enable();
            CombatCard card = dm.GetHandCard(id);
            cardName.text = card.name;
            cardImage.sprite = card.icon;
        }
    }

    public void Enable()
    {
        cardName.gameObject.SetActive(true);
        cardImage.gameObject.SetActive(true);
    }

    public void Disable()
    {
        cardName.gameObject.SetActive(false);
        cardImage.gameObject.SetActive(false);
    }
}
