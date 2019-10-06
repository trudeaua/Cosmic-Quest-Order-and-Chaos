using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class EntityCombat : MonoBehaviour
{
    private EntityStats stats;
    //private CombatClass combatClass;

    private bool isAttacking = false;

    public float attackCooldown = 0.5f;
    public float attackRadius = 2f;


    private void Start()
    {
        stats = GetComponent<EntityStats>();
        //combatClass = stats.combatClass;
    }

    public void PrimaryAttack()
    {
        // TODO very temporary combat architecture
        if (!isAttacking)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        Debug.Log("Attacking!");

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackRadius, Color.yellow);
                // Do damage
                hit.transform.GetComponent<EntityStats>().TakeDamage(stats.baseDamage.GetValue());
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackRadius, Color.white);
            Debug.Log("Nothing to hit!");
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
