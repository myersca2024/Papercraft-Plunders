using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public enum DamageType
{
    DEFAULT
}

[CreateAssetMenu(fileName = "CombatCardEffect", menuName = "Scripts/CombatCardEffect", order = 2)]
public class CombatCardEffect : ScriptableObject
{
    public int damage;
    public int healing;
    public DamageType damageType;
    public int duration;
    public bool multidirectional;
    public GameObject visualEffect;
    public Array2DBool pattern;
}
