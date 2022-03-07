using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasContainer : MonoBehaviour
{
    public GameObject container;

    private DoorTriggerBehavior dtb;

    private void Update()
    {
        if (container.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { UseRoomCard(0); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { UseRoomCard(1); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { UseRoomCard(2); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { UseRoomCard(3); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { UseRoomCard(4); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { UseRoomCard(5); }
        }
    }

    public void StartRoomUI(DoorTriggerBehavior dtb)
    {
        this.dtb = dtb;
        container.SetActive(true);
        PlayerController.freeze = true;
    }

    public void UseRoomCard(int index)
    {
        DungeonDeckManager ddm = FindObjectOfType<DungeonDeckManager>();
        if (index < ddm.deck.Count)
        {
            dtb.StartMakeRoom(index);
            container.SetActive(false);
            PlayerController.freeze = false;
        }
    }
}
