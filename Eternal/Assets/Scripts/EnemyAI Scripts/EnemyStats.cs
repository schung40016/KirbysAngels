using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health;
    public GameObject expPoint;
    private AudioSource audioPlayer;
    [SerializeField] AudioClip hurtClip;

    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        audioPlayer.clip = hurtClip;
        audioPlayer.Play();
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
