using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTriAttackCombatController : EnemyCombatController
{
    [SerializeField] protected float secondaryAttackCooldown = 1f;
    [SerializeField] protected float secondaryAttackDelay = 0.6f;
    [SerializeField] protected AudioSource secondaryAttackFx;
    [SerializeField] protected float tertiaryAttackCooldown = 1f;
    [SerializeField] protected float tertiaryAttackDelay = 0.6f;
    [SerializeField] protected AudioSource tertiaryAttackFx;
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

    public override void TertiaryAttack()
    {
        if (AttackCooldown > 0f)
            return;

        Anim.SetTrigger("TertiaryAttack");

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), tertiaryAttackDelay));
        }

        AttackCooldown = tertiaryAttackCooldown;
    }
}
