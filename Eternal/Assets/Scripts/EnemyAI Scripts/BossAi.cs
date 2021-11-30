using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossAi : EnemyAI
{
    private string[] attackAnimations = new string[] { "attack1", "attack2", "attack3" };

    System.Random r = new System.Random();

    protected override void AttackPlayer()
    {
        // Make sure enemy stays still when trying to attack player.
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            int pick = r.Next(2);
            Attack(attackAnimations[pick]);        
        }
    }

    private void Attack(string AttackAnim)
    {
        animator.SetTrigger(AttackAnim);

        // Initiate attack code.
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * fowardVelocity, ForceMode.Impulse);
        rb.AddForce(transform.up * upwardVelocity, ForceMode.Impulse);

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);        // Ensures our enemy won't deal infinite damage to the player.
    }
}
