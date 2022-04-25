using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomCardViewer : MonoBehaviour
{
    public List<RoomCardUI> cardUIList;
    public DungeonDeckManager dm;

    public TMP_Text displayName;
    public TMP_Text displayDescription;
    public TMP_Text displayDifficulty;
    public TMP_Text displayRewards;

    private void Start()
    {
        DrawDisplayCard(0);
    }

    void Update()
    {
        foreach (RoomCardUI rcUI in cardUIList)
        {
            if (rcUI.id < dm.deck.Count)
            {
                rcUI.gameObject.SetActive(true);
                rcUI.DrawCardToUI(dm.deck[rcUI.id]);
            }
            else
            {
                rcUI.gameObject.SetActive(false);
            }
        }
    }

    public void DrawDisplayCard(int id)
    {
        if (id < dm.deck.Count)
        {
            displayName.text = dm.deck[id].name;
            displayDescription.text = dm.deck[id].effectText;
            displayDifficulty.text = dm.deck[id].challengeRating.ToString();
            displayRewards.text = dm.deck[id].numberOfRewards.ToString();
        }
    }
}
