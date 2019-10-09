using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : EntityCombat
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public override void PrimaryAttack()
    {
        // TODO temporary combat architecture
        if (attackCooldown <= 0f)
        {
            attackCooldown = 1f / attackRate;

            anim.SetTrigger("PrimaryAttack");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Do damage
                    StartCoroutine(PerformDamage(hit.transform.GetComponent<EntityStats>(), 0.6f));
                }
            }
;        }
    }

    private void OnPrimaryAttack(InputValue value)
    {
        // Only trigger attack on button down
        if (value.isPressed)
        {
            PrimaryAttack();
        }
    }
}
