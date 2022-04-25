using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryBossHealth : MonoBehaviour
{
    public GameObject[] splitEnemy = new GameObject[5];
    public Vector3 offset;

    private Color color;
    private GridObject go;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        go = FindObjectOfType<GridObject>();
        List<Material> m = new List<Material>();
        gameObject.GetComponentInChildren<SpriteRenderer>().GetMaterials(m);
        material = m[0];
        color = material.color;
        offset.x = offset.x * go.cellSize;
        offset.z = offset.z * go.cellSize;
    }

    public void ResetColor()
    {
        material.SetColor("_Color", color);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox")
        {
            Hitbox hb;
            other.gameObject.TryGetComponent<Hitbox>(out hb);
            int nextEnemy = Random.Range(0, 5);
            Instantiate(splitEnemy[nextEnemy], transform.position + offset, transform.rotation);
        }
    }
}
