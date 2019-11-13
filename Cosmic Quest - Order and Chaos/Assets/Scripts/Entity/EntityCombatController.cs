using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStatsController))]
public class EntityCombatController : MonoBehaviour
{
    protected EntityStatsController Stats;
    protected Animator Anim;
    protected float AttackCooldown;

    private void Awake()
    {
        Stats = GetComponent<EntityStatsController>();
        Anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Reduce attack cooldown counter
        AttackCooldown -= Time.deltaTime;
    }

    protected IEnumerator PerformDamage(EntityStatsController targetStats, int damageValue, float damageDelay)
    {
        yield return new WaitForSeconds(damageDelay);

        // Applies damage to targetStats
        targetStats.TakeDamage(Stats, damageValue);
    }
}
