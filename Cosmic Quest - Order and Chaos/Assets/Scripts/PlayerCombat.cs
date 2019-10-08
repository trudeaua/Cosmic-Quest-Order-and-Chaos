using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : EntityCombat
{
    public override void PrimaryAttack()
    {
        // TODO temporary combat architecture
        if (!isCoolingDown)
        {
            Debug.Log(name + " has attacked!");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Do damage
                    hit.transform.GetComponent<EntityStats>().TakeDamage(stats, stats.baseDamage.GetValue());
                }
            }

            StartCoroutine("AttackCooldown");
        }
    }

    private void OnPrimaryAttack(InputValue value)
    {
        Debug.Log("Attack pressed");

        // Only trigger attack on button down
        if (value.isPressed)
        {
            PrimaryAttack();
        }
    }
}
