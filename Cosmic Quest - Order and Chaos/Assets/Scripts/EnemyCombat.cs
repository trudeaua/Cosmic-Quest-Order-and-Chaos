using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : EntityCombat
{
    public override void PrimaryAttack()
    {
        // TODO temporary combat architecture
        if (!isCoolingDown)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    // Do damage to player
                    hit.transform.GetComponent<EntityStats>().TakeDamage(stats, stats.baseDamage.GetValue());
                }
            }

            StartCoroutine("AttackCooldown");
        }
    }
}
