using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    [SerializeField] GameObject body;
    Rigidbody virtualBody;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        virtualBody = body.GetComponent<Rigidbody>();
        virtualBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Play animation here for death or hurt.

        if (currentHealth <= 0)
        {
            Debug.Log("Dummy died");
            body.SetActive(false);
        }
    }
}
