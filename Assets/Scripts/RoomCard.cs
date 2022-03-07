using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatCard", menuName = "Scripts/RoomCard", order = 2)]
public class RoomCard : ScriptableObject
{
    public new string name;
    public string effectText;
    public int challengeRating;
    public int numberOfEnemies;
    public GameObject[] potentialEnemies;
    public bool containsChest;
    public CombatCard[] combatCardRewards;
    public RoomCard[] roomCardRewards;
}
