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
        // NOTE: Bug here - possibility of dropping nothing if one list is empty
        for (int i = 0; i < numRewards; i++)
        {
            int list;
            if (ccs.Length == 0) { list = 1; }
            else if (rcs.Length == 0) { list = 0; }
            else { list = Random.Range(0, 2); }

            if (list == 0)
            {
                int reward = Random.Range(0, ccs.Length);
                combatCards.Add(ccs[reward]);
                Debug.Log("Combat card added");
            }
            else
            {
                int reward = Random.Range(0, rcs.Length);
                roomCards.Add(rcs[reward]);
                Debug.Log("Room card added");
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
