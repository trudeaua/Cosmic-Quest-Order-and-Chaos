using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStatsController))]
public class EntityCombatController : MonoBehaviour
{
    protected EntityStatsController Stats;
    protected Animator Anim;
    protected float AttackCooldown = 0f;

    public float attackRate = 1f;
    public float attackRadius = 2f;

    private void Awake()
    {
        Stats = GetComponent<EntityStatsController>();
        Anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        AttackCooldown -= Time.deltaTime;
    }

    protected IEnumerator PerformDamage(EntityStatsController targetStats, float damageDelay)
    {
        yield return new WaitForSeconds(damageDelay);

        targetStats.TakeDamage(Stats, Stats.baseDamage.GetValue());
    }
}
