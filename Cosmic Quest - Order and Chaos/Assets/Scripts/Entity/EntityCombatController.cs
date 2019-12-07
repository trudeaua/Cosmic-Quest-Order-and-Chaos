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

    protected IEnumerator CreateVFX(GameObject vfxPrefab, Transform transform, Quaternion rotation, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        GameObject vfx = Instantiate(vfxPrefab, transform.position, rotation);

        var ps = GetFirstPS(vfx);

        Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax + 1);
    }

    private ParticleSystem GetFirstPS(GameObject vfx)
    {
        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps == null && vfx.transform.childCount > 0)
        {
            foreach (Transform t in vfx.transform)
            {
                ps = t.GetComponent<ParticleSystem>();
                if (ps != null)
                    return ps;
            }
        }
        return ps;
    }
}
