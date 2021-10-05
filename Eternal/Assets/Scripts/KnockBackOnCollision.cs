using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackOnCollision : MonoBehaviour
{
    [SerializeField] private float knockbackStrength;

    // Program detects an object then acts upon.
    public void OnCollisionEnter(Collision collision)
    {
        // Obtain the enemy's virtual body.
        Rigidbody body = collision.collider.GetComponent<Rigidbody>();

        if (body != null)
        {
            Vector3 direction = collision.transform.position - transform.position;

            body.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }
    }
}
