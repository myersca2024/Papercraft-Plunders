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
        if (dm == null) { dm = FindObjectOfType<DeckManager>(); }
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
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
