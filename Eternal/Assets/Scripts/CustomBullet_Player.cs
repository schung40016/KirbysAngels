using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet_Player : CustomBullet
{
    protected override void Explode()
    {
        if (explosion != null)
        {
            impactGo = Instantiate(explosion, transform.position, Quaternion.identity);
        }

        // Check all objects caught within the explosion.
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log("Hello");
            // Get component of target and call take Damage.
            enemies[i].GetComponent<EnemyStats>().TakeDamage(explosionDamage);
        }

        Invoke("Delay", 0.01f);
    }
}
