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

    protected IEnumerator CreateVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        GameObject vfx = Instantiate(vfxPrefab, position, rotation);

        var ps = GetFirstPS(vfx);

        Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax + 1);
    }

    protected IEnumerator CreateVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation, Color vfxColour, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        GameObject vfx = Instantiate(vfxPrefab, position, rotation);

        ParticleSystem[] particleSystems = vfx.GetComponentsInChildren<ParticleSystem>();
        // Convert the hue of the particle effect to the hue of the character colour
        foreach (ParticleSystem p in particleSystems)
        {
            ParticleSystem.MainModule main = p.main;
            float h1, s1, v1, h2, s2, v2, h3, s3, v3;
            Color min = vfxColour;
            Color max = vfxColour;
            float minA = main.startColor.colorMin.a;
            float maxA = main.startColor.colorMax.a;
            if (vfxColour != Color.white && vfxColour != Color.black && vfxColour != Color.gray)
            {
                // need to maintain alpha values, they are forgotten in the RGB to HSV to RGB conversion
                // need to get saturation and brightness value
                Color.RGBToHSV(main.startColor.colorMin, out h1, out s1, out v1);
                Color.RGBToHSV(main.startColor.colorMax, out h2, out s2, out v2);
                Color.RGBToHSV(vfxColour, out h3, out s3, out v3);
                min = Color.HSVToRGB(h3, s1, v1);
                max = Color.HSVToRGB(h3, s2, v2);
            }
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(min.r, min.g, min.b, minA), new Color(max.r, max.g, max.b, maxA));
        }

        ParticleSystem ps = GetFirstPS(vfx);

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

    protected virtual IEnumerator Spawn(GameObject obj, float speed = 0.05f, float delay = 0f)
    {
        Collider col = obj.GetComponent<Collider>();
        float from = -1 * col.bounds.center.y * 4;
        float to = 0;
        col.enabled = false;
        obj.transform.position = new Vector3(obj.transform.position.x, from, obj.transform.position.z);
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        Anim.SetTrigger("Spawn");
        float offset = 0;
        while (obj.transform.position.y < to)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, from + offset, obj.transform.position.z);
            offset += speed;
            yield return new WaitForSeconds(0.01f);
        }
        col.enabled = true;
    }
}
