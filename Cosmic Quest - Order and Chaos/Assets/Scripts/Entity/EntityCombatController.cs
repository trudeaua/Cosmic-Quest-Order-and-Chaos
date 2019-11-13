using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStatsController))]
public class EntityCombatController : MonoBehaviour
{
    protected EntityStatsController Stats;
    protected Animator Anim;
    protected float AttackCooldown;
    
    // Entity layer mask constant for entity raycasting checks
    protected const int EntityLayer = 1 << 9;

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
    
    protected IEnumerator LaunchProjectile(GameObject projectile, Vector3 direction, float launchDelay)
    {
        yield return new WaitForSeconds(launchDelay);
        
        // Launch projectile from projectile pool
        Debug.Log("Projectile launched!");
    }
}
