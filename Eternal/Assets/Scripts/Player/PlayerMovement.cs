using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Physics
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 5f;       // 0.4f
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    private bool isBlocked = false;

    private float forwardInput;
    private float horizontalInput;

    // Combo & Animation
    Animator animator;
    ControlManager controlManager;
    int currentComboPriority = 0;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public GameObject hurtB;
    private Transform playerTrans;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject expShop;
    private int damageTier; 

    // Start is called before the first frame update
    void Awake()
    {
        playerTrans = GetComponent<Transform>();
        if (controlManager == null)
        {
            controlManager = FindObjectOfType<ControlManager>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Creates a sphere hitbox which checks if the player is grounded or not.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!isBlocked) // As long as the player is not stuck in a fighting animation, we can let the player move.
        {
            // Move player.
            forwardInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            Vector3 move = transform.right * horizontalInput + transform.forward * forwardInput;
            controller.Move(move * speed * Time.deltaTime);
            animator.SetFloat("SideRunSpeed", horizontalInput);
            animator.SetFloat("RunSpeed", forwardInput);

            // Jump!!
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jump");
            }

            // Responsible for applying gravity to the player.
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    // Executes the player's move.
    public void PlayMove(Moves move, int comboPriority, int damage, float moveWaitTime, float knockBackMultiplier, Vector3 knockBackDirection, int manaUsage)
    {
        if (Moves.None != move && !isBlocked)
        {
            if (comboPriority > currentComboPriority)
            {
                currentComboPriority = comboPriority;
                //ResetTriggers();
                controlManager.ResetCombo();
            }
            else
            {
                return;
            }

            if (player.currentMana - manaUsage > 0)
            {
                // Use Switch statements to handle animation and call the Attack Move.
                switch (move)
                {
                    case Moves.Punch:
                        animator.SetTrigger("Punch");
                        break;
                    case Moves.Kick:
                        animator.SetTrigger("Kick");
                        break;
                    case Moves.Uppercut:
                        animator.SetTrigger("UpperCut");
                        break;
                    case Moves.Windblast:
                        Debug.Log("Wind blast");
                        break;
                }
                StartCoroutine(StopPlayerInput(moveWaitTime));

                damageTier = expShop.GetComponent<ExperienceShop>().GetDamageTier();

                Attack(damage * damageTier, knockBackMultiplier, knockBackDirection);
                player.UseMana(manaUsage);
            }

            currentComboPriority = 0;
        }
    }

    void ResetTriggers()
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.ResetTrigger(parameter.name);
        }
    }

    // Handles damaging the enemy caught within player's hurtbox.
    void Attack(int damage, float knockBackMultiplier, Vector3 knockBackDirection)
    {
        // Check to see if anyone was in range.
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Apply damage.
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name + " for " + damage + " health points.");
            enemy.GetComponent<EnemyStats>().TakeDamage(damage);

            // Handles enemy knock back.
            enemy.GetComponentInChildren<ImpactReceiver>().AddImpact(playerTrans.forward, 50f);
        }
    }


    // Draws the hurtbox of the Player's range.
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Disables player input for a given time.
    IEnumerator StopPlayerInput(float moveTime)
    {
        isBlocked = true;
        yield return new WaitForSeconds(moveTime);
        isBlocked = false;
    }

}
