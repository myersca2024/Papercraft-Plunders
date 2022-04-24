using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public int x;
    public int y;
    public GridObject go;

    private void Awake()
    {
        Destroy(this.gameObject, 1);
    }

    private void Start()
    {
        go.DeactivateCell(x, y);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.tag != "Player" && other.tag != "Enemy" && other.tag != "Ground" && other.tag != "Hitbox" && other.tag != "CollisionDetector" && other.tag != "DoorTrigger")
        {
            go.SetGridValue(x, y, true);
        }
    }
}
