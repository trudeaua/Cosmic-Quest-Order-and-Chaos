using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCombatController : EntityCombatController
{
    public float attackCooldown = 1f;
    public float attackRadius = 3f;
    public float attackAngle = 45f;

    protected List<GameObject> Players;

    private void Start()
    {
        Players = PlayerManager.players;
    }

    public virtual void PrimaryAttack()
    {
        if (AttackCooldown > 0f)
            return;
        
        AttackCooldown = attackCooldown;

        Anim.SetTrigger("Stab Attack");

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), 0.6f));
        }
    }
    
    /// <summary>
    /// Determines if the enemy can deal damage to a player
    /// </summary>
    /// <param name="target">The position of the target player</param>
    /// <param name="radius">The range of the attack</param>
    /// <param name="sweepAngle">The angular distance in degrees of the attacks FOV.
    /// If set to 360 or left unset then the enemy can attack in any direction.</param>
    /// <returns>Whether the enemy can damage the player</returns>
    protected bool CanDamageTarget(Vector3 target, float radius, float sweepAngle = 360f)
    {
        // TODO need to rethink hitboxes or standardize projecting from y = 1
        Vector3 pos = transform.position;
        pos.y = 1f;
        Vector3 rayDirection = target - pos;
        rayDirection.y = 0;

        if (Mathf.Approximately(sweepAngle, 360f) || Vector3.Angle(rayDirection, transform.forward) <= sweepAngle * 0.5f)
        {
            // Check if enemy is within player's sight
            if (Physics.Raycast(pos, rayDirection, out RaycastHit hit, radius))
            {
                return hit.transform.CompareTag("Player");
            }
        }

        return false;
    }
}
