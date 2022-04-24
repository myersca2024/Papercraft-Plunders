using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBehavior : MonoBehaviour
{
    public List<CombatCard> combatCards;
    public List<RoomCard> roomCards;
    public CCEditUI editUI;

    private DeckManager dm;
    private DungeonDeckManager ddm;

    private void Start()
    {
        dm = FindObjectOfType<DeckManager>();
        ddm = FindObjectOfType<DungeonDeckManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (RoomCard rc in roomCards)
            {
                ddm.AddCard(rc);
            }

            dm.ShuffleDiscardToDeck();
            editUI.SetChest(this);
            PlayerController.freeze = true;
            editUI.gameObject.SetActive(true);
        }
    }

    public void SetRewards(int numRewards, CombatCard[] ccs, RoomCard[] rcs)
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            
        }
    }
}
