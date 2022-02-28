using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatCard", menuName = "Scripts/CombatCard", order = 1)]
public class CombatCard : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public CombatCardEffect effect;
    public string effectText;
}
