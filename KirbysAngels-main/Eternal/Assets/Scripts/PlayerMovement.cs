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
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // Combo & Animation
    Animator animator;
    ControlManager controlManager;
    int currentComboPriority = 0;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Awake()
    {
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

        // Move player.
        float forwardInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * horizontalInput + transform.forward * forwardInput;
        controller.Move(move * speed * Time.deltaTime);
        animator.SetFloat("SideRunSpeed", horizontalInput);
        animator.SetFloat("RunSpeed", forwardInput);

        // Jump!!
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.enabled = false;
            animator.enabled = true;
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Responsible for applying gravity to the player.
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void PlayMove(Moves move, int comboPriority, int damage)
    {
        if (Moves.None != move)
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
            }
            Attack(damage);
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

    void Attack(int damage)
    {
        // Check to see if anyone was in range.
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Apply damage.
        foreach(Collider enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name + " for " + damage + " health points.");
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
