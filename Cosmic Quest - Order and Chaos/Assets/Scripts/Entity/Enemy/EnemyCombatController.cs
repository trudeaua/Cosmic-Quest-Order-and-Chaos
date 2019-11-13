using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : EntityCombatController
{
    public float attackRate = 1f;
    public float attackRadius = 2f;
    
    public void PrimaryAttack()
    {
        // TODO temporary combat architecture
        if (AttackCooldown <= 0f)
        {
            AttackCooldown = 1f / attackRate;

            Anim.SetTrigger("Stab Attack");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    // Do damage to player
                    StartCoroutine(PerformDamage(hit.transform.GetComponent<EntityStatsController>(), Stats.damage.GetValue(), 0.6f));
                }
            }
        }
    }
}
