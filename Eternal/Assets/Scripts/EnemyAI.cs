using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;              // Checks if our AI can walk at a certain spot.
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // States 
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Initialize variables.
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check whether player is in sight or in attack range.
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();      // Did not see a player, stroll.
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();      // Did not see a player, stroll.
        if (playerInSightRange && playerInAttackRange) AttackPlayer();      // Did not see a player, stroll.

    }

    private void Patrolling()
    {
        if (!walkPointSet) 
            SearchWalkPoint();

        // Move enemy to the point.
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Using calculated distance, check if enemy reached the new position.
        if (distanceToWalkPoint.magnitude < 1f) 
            walkPointSet = false;
    }

    // Determines by random where the enemy should walk around.
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        // Reposition enemy.
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Using a raycast check if the enemy can walk on the ground (not midair). 
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround));
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Make sure enemy stays still when trying to attack player.
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Initiate attack code.
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);        // Ensures our enemy won't deal infinite damage to the player.
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

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
        Destroy(gameObject);
    }

    // Draws hitbox for attack.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
