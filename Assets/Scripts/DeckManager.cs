using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CombatCard> deck;
    public float drawTime;
    public int activeCard = 0;
    [HideInInspector] public int deckSize = 0;
    [HideInInspector] public int handSize = 0;
    [HideInInspector] public int discardSize = 0;

    private List<CombatCard> hand;
    private List<CombatCard> discard;
    private float currTime;

    void Start()
    {
        hand = new List<CombatCard>();
        discard = new List<CombatCard>();

        deckSize = deck.Count;
        ShuffleDeck();
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }

        currTime = drawTime;
    }

    private void Update()
    {
        if (deckSize == 0)
        {
            ShuffleDiscardToDeck();
        }

        if (currTime >= drawTime && handSize < 5)
        {
            DrawCard();
            currTime = 0;
        }

        currTime += Time.deltaTime;
    }

    // Found on Stack Overflow:
    // https://stackoverflow.com/questions/2094239/swap-two-items-in-listt
    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    void DrawCard()
    {
        CombatCard card = deck[0];
        hand.Add(card);
        handSize++;
        deck.RemoveAt(0);
        deckSize--;
    }

    public void DiscardCard(int index)
    {
        CombatCard card = hand[index];
        discard.Add(card);
        discardSize++;
        hand.RemoveAt(index);
        handSize--;
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int r = i + Random.Range(0, deck.Count - i);
            Swap(deck, i, r);
        }
    }

    void ShuffleDiscardToDeck()
    {
        for (int i = 0; i < discard.Count; i++)
        {
            deck.Add(discard[i]);
            deckSize++;
        }
        discard.RemoveRange(0, discard.Count);
        discardSize = discard.Count;
        ShuffleDeck();
    }

    public CombatCard GetHandCard(int index)
    {
        return hand[index];
    }

    public void HighlightCard(int index) {
        activeCard = index;
    }
}
