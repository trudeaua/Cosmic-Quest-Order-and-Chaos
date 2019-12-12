using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDoubleAttackCombatController : EnemyCombatController
{
    [SerializeField] protected float primaryAttackCooldown = 1f;
    [SerializeField] protected float primaryAttackDelay = 0.6f;
    [SerializeField] protected EntityAudioClip primaryAttackSFX;

    [SerializeField] protected float secondaryAttackCooldown = 1f;
    [SerializeField] protected float secondaryAttackDelay = 0.6f;
    [SerializeField] protected EntityAudioClip secondaryAttackSFX;

    public override void PrimaryAttack()
    {
        if (AttackCooldown > 0f)
            return;
        
        StartCoroutine(Stats.PlayAudioOverlap(primaryAttackSFX));
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

        StartCoroutine(Stats.PlayAudioOverlap(secondaryAttackSFX));
        Anim.SetTrigger("SecondaryAttack");

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), secondaryAttackDelay));
        }

        AttackCooldown = secondaryAttackCooldown;
    }
}
