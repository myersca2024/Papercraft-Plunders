using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCEditUI : MonoBehaviour
{
    public List<CCEditCard> inventoryCards;
    public List<CCEditCard> chestCards;

    private DeckManager dm;
    private TreasureBehavior chest;

    void Start()
    {
        dm = FindObjectOfType<DeckManager>();
    }

    private void Update()
    {
        foreach (CCEditCard ic in inventoryCards)
        {
            if (ic.index < dm.handSize - 1)
            {
                ic.gameObject.SetActive(true);
                ic.UpdateCardAtIndex(dm.GetHandCard(ic.index + 1));
            }
            else if (ic.index < (dm.handSize - 1) + dm.deckSize)
            {
                ic.gameObject.SetActive(true);
                ic.UpdateCardAtIndex(dm.deck[ic.index - 2]);
            }
            else
            {
                ic.gameObject.SetActive(false);
            }
        }

        if (chest != null)
        {
            foreach (CCEditCard cc in chestCards)
            {
                if (cc.index < chest.combatCards.Count)
                {
                    cc.gameObject.SetActive(true);
                    cc.UpdateCardAtIndex(chest.combatCards[cc.index]);
                }
                else
                {
                    cc.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ChestToDeck(int index)
    {
        if (dm.AddCardToDeck(chest.combatCards[index]))
        {
            chest.combatCards.RemoveAt(index);
        }
    }

    public void DeckToChest(int index)
    {
        if (index < 2 && chest.combatCards.Count < 9)
        {
            chest.combatCards.Add(dm.GetHandCard(index + 1));
            dm.RemoveCardFromDeck(index + 1, true);
        }
        else if (index >= 2 && index < dm.initialDeckSize && chest.combatCards.Count < 9)
        {
            chest.combatCards.Add(dm.deck[index - 2]);
            dm.RemoveCardFromDeck(index - 2, false);
        }
    }

    public void CloseWindow()
    {
        if (dm.deckSize + dm.handSize - 1 == 9)
        {
            PlayerController.freeze = false;
            Destroy(chest.gameObject);
            Time.timeScale = 1;
            this.gameObject.SetActive(false);
        }
    }

    public void SetChest(TreasureBehavior chest)
    {
        this.chest = chest;
    }
}
