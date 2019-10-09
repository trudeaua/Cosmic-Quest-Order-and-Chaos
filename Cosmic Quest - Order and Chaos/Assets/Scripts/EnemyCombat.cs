using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : EntityCombat
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

            anim.SetTrigger("Stab Attack");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    // Do damage to player
                    StartCoroutine(PerformDamage(hit.transform.GetComponent<EntityStats>(), 0.6f));
                }
            }
        }
    }
}
