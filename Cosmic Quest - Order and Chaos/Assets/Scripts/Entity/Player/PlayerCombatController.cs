using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : EntityCombatController
{
    protected virtual void PrimaryAttack()
    {
        // Implement me
        Debug.Log("Player's base primary attack triggered");

        /*if (attackCooldown <= 0f)
        {
            attackCooldown = 1f / attackRate;

            Anim.SetTrigger("PrimaryAttack");

            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Do damage
                    StartCoroutine(PerformDamage(hit.transform.GetComponent<EntityStatsController>(), 0.6f));
                }
            }
        }*/
    }

    protected virtual void SecondaryAttack()
    {
        // Implement me
        Debug.Log("Player's base secondary attack triggered");
    }

    protected virtual void UltimateAbility()
    {
        // Implement me
        Debug.Log("Player's base ultimate ability triggered");
    }

    protected virtual void OnPrimaryAttack(InputValue value)
    {
        // Only trigger attack on button down by default
        if (value.isPressed)
        {
            PrimaryAttack();
        }
    }
    
    protected virtual void OnSecondaryAttack(InputValue value)
    {
        // Only trigger attack on button down by default
        if (value.isPressed)
        {
            SecondaryAttack();
        }
    }

    protected virtual void OnUltimateAbility(InputValue value)
    {
        // Only trigger ability on button down by default
        if (value.isPressed)
        {
            UltimateAbility();
        }
    }
}
