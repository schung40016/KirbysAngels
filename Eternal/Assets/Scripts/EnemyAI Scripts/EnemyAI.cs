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

    // Audio
    protected AudioSource audioPlayer;
    [SerializeField] protected AudioClip[] audioPlayList;
    protected bool playPatrol = true;
    protected bool playChase = true;
    protected bool playAttack = true;

    // player object
    ImpactReceiver playerObjImpact;

    // Animation related.
    protected Animator animator;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;              // Checks if our AI can walk at a certain spot.
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    protected bool alreadyAttacked;
    public GameObject projectile;

    // States 
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // For Knockback physics
    bool knockBack;
    public Vector3 direction;

    // Initialize projectile variables.
    [SerializeField] protected float upwardVelocity = 0.0f;
    [SerializeField] protected float fowardVelocity = 32.0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerObjImpact = player.GetComponentInChildren<ImpactReceiver>();
        audioPlayer = GetComponent<AudioSource>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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

        animator.SetBool("isWalking", true);

        if (!playerInSightRange && !playerInAttackRange)
        {
            if (playPatrol)
            {
                SoundControl(audioPlayList[0], ref playPatrol, ref playChase, ref playAttack);
            }
            Patrolling();      // Did not see a player, stroll.
        }
        if (playerInSightRange && !playerInAttackRange) 
        {
            if (playChase)
            {
                SoundControl(audioPlayList[1], ref playChase, ref playPatrol, ref playAttack);
            }
            ChasePlayer();      // Did not see a player, stroll.
        }
        if (playerInSightRange && playerInAttackRange)
        {
            if (playAttack)
            {
                SoundControl(audioPlayList[2], ref playAttack, ref playPatrol, ref playChase);
            }
            animator.SetBool("isWalking", false);
            AttackPlayer();      // Did not see a player, stroll.
        }

        if (knockBack)
        {
            agent.velocity = direction * 8;
        }

    }   
 
    private void SoundControl(AudioClip audioPlayClip, ref bool notPlay, ref bool play1, ref bool play2)
    {
        audioPlayer.clip = audioPlayClip;
        audioPlayer.Play();
        notPlay = false;
        play1 = true;
        play2 = true;
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

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

    protected virtual void AttackPlayer()
    {
        // Make sure enemy stays still when trying to attack player.
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("isAttacking");

            // Initiate attack code.
            Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * fowardVelocity, ForceMode.Impulse);
            rb.AddForce(transform.up * upwardVelocity, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);        // Ensures our enemy won't deal infinite damage to the player.
        }
    }

    protected void ResetAttack()
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
