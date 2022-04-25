using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDeckManager : MonoBehaviour
{
    public List<RoomCard> deck;

    public void AddCard(RoomCard card)
    {
        if (deck.Count <= 18 && card.name != "Default Room")
        {
            deck.Add(card);
        }
    }

    public void DiscardCard(int index)
    {
        deck.RemoveAt(index);
    }
}
