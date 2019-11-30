using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStatsController))]
public class EntityCombatController : MonoBehaviour
{
    protected EntityStatsController Stats;
    protected Animator Anim;
    protected float AttackCooldown;

    protected Collider[] Hits = new Collider[32];
    
    protected virtual void Awake()
    {
        Stats = GetComponent<EntityStatsController>();
        Anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        // Reduce attack cooldown counter
        if (AttackCooldown > 0f)
            AttackCooldown -= Time.deltaTime;
    }

    protected IEnumerator PerformDamage(EntityStatsController targetStats, float damageValue, float damageDelay = 0f)
    {
        if (damageDelay > 0f)
            yield return new WaitForSeconds(damageDelay);

        // Applies damage to targetStats
        targetStats.TakeDamage(Stats, damageValue);
    }
    
    protected IEnumerator PerformExplosiveDamage(EntityStatsController targetStats, float maxDamage, float stunTime, 
        float explosionForce, Vector3 explosionPoint, float explosionRadius, float explosionDelay = 0f)
    {
        if (explosionDelay > 0f)
            yield return new WaitForSeconds(explosionDelay);

        // Applies damage to targetStats
        targetStats.TakeExplosionDamage(Stats, maxDamage, stunTime, explosionForce, explosionPoint, explosionRadius);
    }
    
    protected IEnumerator LaunchProjectile(GameObject projectilePrefab, Vector3 direction, float launchForce, float range, float launchDelay = 0f)
    {
        if (launchDelay > 0f)
            yield return new WaitForSeconds(launchDelay);
        
        // Launch projectile from projectile pool
        GameObject projectile = ObjectPooler.Instance.GetPooledObject(projectilePrefab);
        projectile.GetComponent<Projectile>().Launch(Stats, direction, launchForce, range);
    }
}
