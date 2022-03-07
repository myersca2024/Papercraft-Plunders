using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomCardUI : MonoBehaviour
{
    public int id;
    public DungeonDeckManager dm;
    public TMP_Text cardName;
    public TMP_Text cardText;

    void Start()
    {
        dm = FindObjectOfType<DungeonDeckManager>();
    }

    void Update()
    {
        if (id + 1 > dm.deck.Count)
        {
            Disable();
        }
        else
        {
            Enable();
            RoomCard card = dm.deck[id];
            cardName.text = card.name;
            cardText.text = card.effectText;
        }
    }

    public void Enable()
    {
        cardName.gameObject.SetActive(true);
        cardText.gameObject.SetActive(true);
    }

    public void Disable()
    {
        cardName.gameObject.SetActive(false);
        cardText.gameObject.SetActive(false);
    }
}
