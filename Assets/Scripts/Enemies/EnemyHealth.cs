using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private GridObject go;

    // Start is called before the first frame update
    void Start()
    {
        go = FindObjectOfType<GridObject>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            //print("Enemy slain!");
            Vector2Int enemyPos = go.GetGrid().GetXY(this.transform.position);
            go.GetGrid().SetValue(enemyPos.x, enemyPos.y, false);
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
