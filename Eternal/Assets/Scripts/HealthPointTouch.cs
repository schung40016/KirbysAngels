using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointTouch : MonoBehaviour
{
    private PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player_Tag")
        {
            player = other.GetComponent<PlayerController>();
            player.currentHealth += 50;
            if (player.currentHealth > player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
