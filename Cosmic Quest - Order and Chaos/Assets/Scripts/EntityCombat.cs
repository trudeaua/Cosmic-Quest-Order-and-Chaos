using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class EntityCombat : MonoBehaviour
{
    protected EntityStats stats;
    //protected CombatClass combatClass;

    protected bool isCoolingDown = false;

    public float attackCooldown = 0.5f;
    public float attackRadius = 2f;


    private void Start()
    {
        stats = GetComponent<EntityStats>();
        //combatClass = stats.combatClass;
    }

    public virtual void PrimaryAttack()
    {
        if (!isCoolingDown)
        {
            Debug.Log("Attacking!");
        }
    }

    IEnumerator AttackCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(attackCooldown);
        isCoolingDown = false;
    }
}
