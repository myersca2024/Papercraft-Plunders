using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public int x;
    public int y;

    private GridObject go;

    private void Awake()
    {
        go = FindObjectOfType<GridObject>();
        Destroy(this.gameObject, 1);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Enemy" && other.tag != "Ground" && other.tag != "Hitbox")
        {
            go.SetGridValue(x, y, true);
        }
    }
}
