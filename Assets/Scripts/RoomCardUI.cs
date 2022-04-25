using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomCardUI : MonoBehaviour
{
    public int id;
    public TMP_Text cardName;
    public TMP_Text cardDiffculty;
    public TMP_Text cardRewards;

    public void DrawCardToUI(RoomCard rc)
    {
        cardName.text = rc.name;
        cardDiffculty.text = rc.challengeRating.ToString();
        cardRewards.text = rc.numberOfRewards.ToString();
    }
}
