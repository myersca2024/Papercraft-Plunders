using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBehavior : MonoBehaviour
{
    public CombatCard[] combatCards;
    public RoomCard[] roomCards;

    private DeckManager dm;
    private DungeonDeckManager ddm;

    private void Start()
    {
        dm = FindObjectOfType<DeckManager>();
        ddm = FindObjectOfType<DungeonDeckManager>();
    }

    public void SetRewards(CombatCard[] ccs, RoomCard[] rcs)
    {
        combatCards = ccs;
        roomCards = rcs;
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
