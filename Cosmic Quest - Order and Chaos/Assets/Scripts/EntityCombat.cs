using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class EntityCombat : MonoBehaviour
{
    protected EntityStats stats;
    //protected CombatClass combatClass;
    protected float attackCooldown = 0f;

    public float attackRate = 1f;
    public float attackRadius = 2f;

    private void Start()
    {
        stats = GetComponent<EntityStats>();
        //combatClass = stats.combatClass;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    public virtual void PrimaryAttack()
    {
        if (attackCooldown <= 0f)
        {
            Debug.Log("Attacking!");
        }
    }

    protected IEnumerator PerformDamage(EntityStats enemyStats, float damageDelay)
    {
        yield return new WaitForSeconds(damageDelay);

        stats.TakeDamage(enemyStats, stats.baseDamage.GetValue());
    }
}
