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
    public TMP_Text uses;
    public Image cardImage;
    public Image highlight;

    private bool highlighted = false;

    private void Start()
    {
        dm = FindObjectOfType<DeckManager>();
        if (id != 0) {
            highlight.gameObject.SetActive(false);
        }
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
            if (id != 0) {
                uses.text = card.GetUses().ToString();
            }
            cardImage.sprite = card.icon;
        }
        if (dm.activeCard == id && !highlighted) {
            highlight.gameObject.SetActive(true);
            highlighted = true;
        }
        if (dm.activeCard != id && highlighted) {
            highlight.gameObject.SetActive(false);
            highlighted = false;
        }
    }

    public void Enable()
    {
        cardName.gameObject.SetActive(true);
        uses.gameObject.SetActive(true);
        cardImage.gameObject.SetActive(true);
    }

    public void Disable()
    {
        cardName.gameObject.SetActive(false);
        uses.gameObject.SetActive(false);
        cardImage.gameObject.SetActive(false);
    }
}
