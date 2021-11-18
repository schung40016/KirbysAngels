using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPointTouch : MonoBehaviour
{
    private PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player_Tag")
        {
            player = other.GetComponent<PlayerController>();
            player.playerExp += 100;
            player.experience.text = "Exp: " + player.playerExp;
            Destroy(gameObject);
        }
    }
}
