using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CCEditCard : MonoBehaviour
{
    public int index;
    public TMP_Text cardName;
    public TMP_Text cardUses;
    public Image cardImage;
    
    public void UpdateCardAtIndex(CombatCard cc)
    {
        cardName.text = cc.name;
        cardUses.text = cc.uses.ToString();
        cardImage.sprite = cc.icon;
    }
}
