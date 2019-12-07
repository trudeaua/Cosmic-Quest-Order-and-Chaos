using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTriAttackCombatController : EnemyCombatController
{
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackDelay = 0.6f;
    public float spellCooldown = 1f;
    public float spellAttackDelay = 0.6f;
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

    public override void SecondaryAttack()
    {
        if (AttackCooldown > 0f)
            return;


        Anim.SetTrigger("SecondaryAttack");

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), secondaryAttackDelay));
        }

        AttackCooldown = secondaryAttackCooldown;
    }

    public override void Spell()
    {
        if (AttackCooldown > 0f)
            return;

        Anim.SetTrigger("Spell");

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), spellAttackDelay));
        }

        AttackCooldown = spellCooldown;
    }
}
