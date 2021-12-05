using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointTouch : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private int healthBoost = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player_Tag")
        {
            player = other.GetComponent<PlayerController>();
            player.currentHealth += healthBoost;
            player.healthBar.SetHealth(player.currentHealth);
            if (player.currentHealth > player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
