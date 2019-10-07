using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : EntityCombat
{
    public override void PrimaryAttack()
    {
        // TODO temporary combat architecture
        if (!isCoolingDown)
        {
            Debug.Log("Attacking!");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Do damage
                    hit.transform.GetComponent<EntityStats>().TakeDamage(stats.baseDamage.GetValue());
                }
            }

            StartCoroutine("AttackCooldown");
        }
    }
}
