using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health;

    public GameObject expPoint;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Invoke(nameof(KillEnemy), 0.5f);
        }
    }

    private void KillEnemy()
    {
        // Spawn a experience point when killing an enemy.
        Instantiate(expPoint, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

        Destroy(gameObject);
    }
}
