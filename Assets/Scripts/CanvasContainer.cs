using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasContainer : MonoBehaviour
{
    public GameObject container;
    public DungeonDeckManager dm;
    public List<RoomCardUI> cardUIList;

    public TMP_Text displayName;
    public TMP_Text displayDescription;
    public TMP_Text displayDifficulty;
    public TMP_Text displayRewards;

    private DoorTriggerBehavior dtb;

    private void Update()
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

    public void StartRoomUI(DoorTriggerBehavior dtb)
    {
        this.dtb = dtb;
        container.SetActive(true);
        DrawDisplayCard(0);
        PlayerController.freeze = true;
    }

    public void UseRoomCard(int index)
    {
        DungeonDeckManager ddm = FindObjectOfType<DungeonDeckManager>();
        if (index < ddm.deck.Count)
        {
            dtb.StartMakeRoom(index);
            container.SetActive(false);
            ddm.DiscardCard(index);
            PlayerController.freeze = false;
        }
    }

    public void DrawDisplayCard(int id)
    {
        displayName.text = dm.deck[id].name;
        displayDescription.text = dm.deck[id].effectText;
        displayDifficulty.text = dm.deck[id].challengeRating.ToString();
        displayRewards.text = dm.deck[id].numberOfRewards.ToString();
}
}
