using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;

    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    public Collider bullCollider;

    protected GameObject impactGo;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        // When to explode our projectile.
        if (collisions > maxCollisions)
        {
            Debug.Log("ini");
            Explode();
        }
        // Countdown the lifetime of the explosive.
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0) Explode();
    }

    protected virtual void Explode()
    {
        if (explosion != null)
        {
            impactGo = Instantiate(explosion, transform.position, Quaternion.identity);
        }

        // Check all objects caught within the explosion.
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get component of target and call take Damage.
            enemies[i].GetComponent<PlayerController>().TakeDamage(explosionDamage);
        }

        Invoke("Delay", 0.01f);
    }

    private void Delay()
    {
        Destroy(impactGo);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Coutn up collisions.
        if (!collision.gameObject.layer.Equals("Enemy"))
            collisions++;

        // Explode if the bullets hits object/enemy directly
        if (collision.collider.CompareTag("Player_Tag") && explodeOnTouch)
        {
            Explode();
        }
    }

    private void Setup()
    {
        // Create physics material.
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        //GetComponent<SphereCollider>().material = physics_mat;
        bullCollider.material = physics_mat;

        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
