using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private PlayerController playerControllerScript;

    //Testing above

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // player object
    ImpactReceiver playerObjImpact;

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

    // For Knockback physics
    bool knockBack;
    public Vector3 direction;

    // Initialize projectile variables.
    [SerializeField] private float upwardVelocity = 0.0f;
    [SerializeField] private float fowardVelocity = 32.0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerObjImpact = player.GetComponentInChildren<ImpactReceiver>();
    }

    private void Start()
    {
        knockBack = false;
    }

    void FixedUpdate()
    {
        // Check whether player is in sight or in attack range.
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();      // Did not see a player, stroll.
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();      // Did not see a player, stroll.
        if (playerInSightRange && playerInAttackRange) AttackPlayer();      // Did not see a player, stroll.

        if (knockBack)
        {
            agent.velocity = direction * 8;
        }
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
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
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
            Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * fowardVelocity, ForceMode.Impulse);
            rb.AddForce(transform.up * upwardVelocity, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);        // Ensures our enemy won't deal infinite damage to the player.
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
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
