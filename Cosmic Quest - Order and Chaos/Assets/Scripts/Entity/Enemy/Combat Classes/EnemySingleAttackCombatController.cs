using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySingleAttackCombatController : EnemyCombatController
{
    public override void PrimaryAttack()
    {
        if (AttackCooldown > 0f)
            return;
        

        Anim.SetTrigger("PrimaryAttack");

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), primaryAttackDelay));
        }

        AttackCooldown = primaryAttackCooldown;
    }
}
