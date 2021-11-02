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

    public float health;

    // player object
    public GameObject playerObj;

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

    // Initialize variables.
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        knockBack = false;
        
    }

    private void Update()
    {

        /*
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
        */

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

    
    /*
        IEnumerator KnockBack()
        {
            knockBack = true;
            agent.speed = 10;
            agent.angularSpeed = 0;         // Keeps agent from spinning.
            agent.acceleration = 20;

            Debug.Log(agent.acceleration);

            yield return new WaitForSeconds(0.2f);      // Knock enemy back for a little bit of time.

            // Knockback finished, return speed to normal.
            knockBack = false;
            agent.speed = 10;
            agent.angularSpeed = 180;         // Keeps agent from spinning.
            agent.acceleration = 10;
        }

        private void OnTriggerEnter(Collider other)
        {   
            direction = other.transform.forward;
            if (other.name.Length == ("HurtBox(Clone)".Length)) 
            {
                StartCoroutine(KnockBack());
                Destroy(other.gameObject);
            }
        }
    */
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

        Debug.Log("To Attack!~~~");
        if (!alreadyAttacked)
        {
            Debug.Log("Attack~~~"+ playerObj.gameObject.tag+"~~~");
            // Initiate attack code.
            Rigidbody rb = playerObj.GetComponent<Rigidbody>();


 
            Vector3 lookDirection =(player.position - transform.position).normalized;

            Vector3 curr_pos = player.position;

            // playerObj.transform.position = new Vector3(100, 100, 100);
            playerObj.transform.position = new Vector3(player.position.x + 5, player.position.y+2, player.position.z + 5);


            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            //rb.AddForce(transform.up * 320000000f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

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
