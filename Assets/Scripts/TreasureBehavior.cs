using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBehavior : MonoBehaviour
{
    public List<CombatCard> combatCards;
    public List<RoomCard> roomCards;

    private DeckManager dm;
    private DungeonDeckManager ddm;

    private void Start()
    {
        dm = FindObjectOfType<DeckManager>();
        ddm = FindObjectOfType<DungeonDeckManager>();
    }

    public void SetRewards(int numRewards, CombatCard[] ccs, RoomCard[] rcs)
    {
        // NOTE: Possibility of dropping nothing if one list is empty
        for (int i = 0; i < numRewards; i++)
        {
            int list = Random.Range(0, 2);
            if (list == 0)
            {
                int reward = Random.Range(0, ccs.Length);
                combatCards.Add(ccs[reward]);
            }
            else
            {
                int reward = Random.Range(0, rcs.Length);
                roomCards.Add(rcs[reward]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (CombatCard cc in combatCards)
            {
                dm.deck.Add(cc);
            }

            foreach (RoomCard rc in roomCards)
            {
                ddm.AddCard(rc);
            }

            Destroy(this.gameObject, 0.1f);
        }
    }
}
