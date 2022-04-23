using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public CombatCard defaultCard;
    public List<CombatCard> hand;
    public List<CombatCard> deck;
    public float shuffleTime;
    public int activeCard = 0;
    public int totalHandSize = 3;
    [HideInInspector] public int deckSize = 0;
    [HideInInspector] public int handSize = 0;
    [HideInInspector] public int discardSize = 0;
    [HideInInspector] public int initialDeckSize;

    private List<CombatCard> discard;
    private float currTime;

    void Start()
    {
        hand = new List<CombatCard>();
        discard = new List<CombatCard>();
        initialDeckSize = deck.Count;

        hand.Add(defaultCard);
        handSize++;

        deckSize = deck.Count;
        ShuffleDeck();
        for (int i = 0; i < totalHandSize - 1; i++)
        {
            DrawCard();
        }

        currTime = shuffleTime;
    }

    private void Update()
    {
        if (deckSize == 0 && handSize == 1)
        {
            currTime = 0;
            ShuffleDiscardToDeck();
        }

        if (handSize < totalHandSize && currTime > shuffleTime && deckSize > 0)
        {
            DrawCard();
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
        if (deckSize > 0)
        {
            CombatCard card = deck[0];
            hand.Add(card);
            handSize++;
            deck.RemoveAt(0);
            deckSize--;
        }
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

    public void ShuffleDiscardToDeck()
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

    public bool AddCardToDeck(CombatCard card)
    {
        if (handSize - 1 + deckSize < initialDeckSize)
        {
            if (handSize < totalHandSize)
            {
                hand.Add(card);
                handSize++;
                return true;
            }
            else
            {
                deck.Add(card);
                deckSize++;
                return true;
            }
        }
        return false;
    }

    public bool RemoveCardFromDeck(int index, bool isInHand)
    {
        if (isInHand && index < handSize && index != 0)
        {
            hand.RemoveAt(index);
            handSize--;
            DrawCard();
            return true;
        }
        else if (!isInHand && index < deckSize)
        {
            deck.RemoveAt(index);
            deckSize--;
            return true;
        }
        return false;
    }

    public CombatCard GetHandCard(int index)
    {
        if (index >= hand.Count) {
            return null;
        }

        return hand[index];
    }

    public void HighlightCard(int index) {
        activeCard = index;
    }
}
