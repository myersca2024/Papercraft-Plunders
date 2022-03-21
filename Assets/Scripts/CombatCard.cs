using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

[CreateAssetMenu(fileName = "CombatCard", menuName = "Scripts/CombatCard", order = 1)]
public class CombatCard : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public string effectText;
    public int damage;
    public int healing;
    public int uses = 1;
    public float duration = 0.1f;
    public bool multidirectional;
    public GameObject visualEffect;
    public Array2DBool pattern;

    private int currentUses = -1;

    public void DecrementUses()
    {
        if (currentUses == -1 || currentUses == 0) RefreshUses();
        currentUses--;
    }

    public int GetUses()
    {
        return currentUses;
    }

    public void RefreshUses()
    {
        currentUses = uses;
    }
}
