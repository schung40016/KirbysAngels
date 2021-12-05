using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossAi : EnemyAI
{
    private string[] attackAnimations = new string[] { "isAttacking", "isAttacking2", "isAttacking3" };
    [SerializeField] GameObject[] attackModes;

    System.Random r = new System.Random();

    protected override void AttackPlayer()
    {
        // Make sure enemy stays still when trying to attack player.
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            int pick = r.Next(0, attackModes.Length);
            Debug.Log(pick);
            Attack(attackAnimations[pick], attackModes[pick]);        
        }
    }

    private void Attack(string AttackAnim, GameObject attackMode)
    {
        animator.SetTrigger(AttackAnim);

        // Initiate attack code.
        Rigidbody rb = Instantiate(attackMode, transform.position + transform.forward * 3, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * fowardVelocity, ForceMode.Impulse);
        
        if (attackMode.name.Equals("BulletSphere"))
        {
            rb.AddForce(transform.up * 0, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * upwardVelocity, ForceMode.Impulse);
        }

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);        // Ensures our enemy won't deal infinite damage to the player.
    }
}
