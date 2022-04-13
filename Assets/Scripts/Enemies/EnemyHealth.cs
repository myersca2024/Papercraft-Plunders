using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private Color color;
    private GridObject go;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        go = FindObjectOfType<GridObject>();
        currentHealth = maxHealth;
        List<Material> m = new List<Material>();
        gameObject.GetComponent<MeshRenderer>().GetMaterials(m);
        material = m[0];
        color = material.color;

        if (!deathSound) {
            deathSound = hitSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
            //print("Enemy slain!");
            Vector2Int enemyPos = go.GetGrid().GetXY(this.transform.position);
            go.GetGrid().SetValue(enemyPos.x, enemyPos.y, false);
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0) {
            AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position);
        }

        material.SetColor("_Color", Color.red);
        Invoke("ResetColor", 0.2f);
    }

    public void ResetColor() {
        material.SetColor("_Color", color);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox")
        {
            Hitbox hb;
            other.gameObject.TryGetComponent<Hitbox>(out hb);
            TakeDamage(hb.damage);
        }
    }
}
