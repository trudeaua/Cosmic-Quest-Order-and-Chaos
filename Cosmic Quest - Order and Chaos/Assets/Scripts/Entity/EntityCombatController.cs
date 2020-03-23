using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStatsController))]
public class EntityCombatController : MonoBehaviour
{
    protected EntityStatsController Stats;
    protected Animator Anim;
    public float AttackCooldown { get; protected set; }

    protected Collider[] Hits = new Collider[32];

    protected AudioSource[] Audio;
    // Audio source for playing vocal audio clips
    protected AudioSource VocalAudio;
    // Audio source for playing weapon audio clips
    protected AudioSource WeaponAudio;
    [SerializeField] protected AudioHelper.EntityAudioClip takeDamageVocalSFX;
    [SerializeField] protected AudioHelper.EntityAudioClip entityDeathVocalSFX;

    protected virtual void Awake()
    {
        Stats = GetComponent<EntityStatsController>();
        Anim = GetComponentInChildren<Animator>();
        Audio = GetComponents<AudioSource>();
        // Assign audio sources for weapon and vocal SFX
        // There should be 2 audio sources to handle both SFX types playing concurrently
        if (Audio.Length == 2)
        {
            WeaponAudio = Audio[0];
            VocalAudio = Audio[1];
        }
        else if (Audio.Length < 2)
        {
            WeaponAudio = Audio[0];
            VocalAudio = Audio[0];
        }
    }

    protected virtual void Update()
    {
        // Reduce attack cooldown counter
        if (AttackCooldown > 0f)
            AttackCooldown -= Time.deltaTime;
    }
    
    /// <summary>
    /// Perform damage on a target
    /// </summary>
    /// <param name="targetStats">Target's stats</param>
    /// <param name="damageValue">Damage value to apply</param>
    /// <param name="damageDelay">Time to wait before damage is applied</param>
    protected IEnumerator PerformDamage(EntityStatsController targetStats, float damageValue, float damageDelay = 0f)
    {
        if (damageDelay > 0f)
            yield return new WaitForSeconds(damageDelay);

        // Applies damage to targetStats
        targetStats.TakeDamage(Stats, damageValue);
    }

    /// <summary>
    /// Perform explosive damage on a target
    /// </summary>
    /// <param name="targetStats">Target's stats</param>
    /// <param name="maxDamage">Maximum damage able to be dealt</param>
    /// <param name="stunTime">Number of seconds to stun damaged targets for</param>
    /// <param name="explosionForce">Force of the explosion</param>
    /// <param name="explosionPoint">Explosion origin</param>
    /// <param name="explosionRadius">Explosion effect radius</param>
    /// <param name="explosionDelay">Number of seconds before explosion affects targets</param>
    protected IEnumerator PerformExplosiveDamage(EntityStatsController targetStats, float maxDamage, float stunTime,
        float explosionForce, Vector3 explosionPoint, float explosionRadius, float explosionDelay = 0f)
    {
        if (explosionDelay > 0f)
            yield return new WaitForSeconds(explosionDelay);

        // Applies damage to targetStats
        targetStats.TakeExplosionDamage(Stats, maxDamage, stunTime, explosionForce, explosionPoint, explosionRadius);
    }

    /// <summary>
    /// Launch a projectile
    /// </summary>
    /// <param name="projectilePrefab">The prefab to use as the projectile</param>
    /// <param name="direction">The direction to launch the projectile in</param>
    /// <param name="launchForce">The force applied to the projectile at launch</param>
    /// <param name="range">Maximum range the projectile can travel for</param>
    /// <param name="damage">The damage the projectile will do</param>
    /// <param name="targetTag">The tag of the target type</param>
    protected void LaunchDamageProjectile(GameObject projectilePrefab, Vector3 direction, float launchForce, float range, float damage, string targetTag = "Enemy")
    {
        // Launch projectile from projectile pool
        GameObject projectile = ObjectPooler.Instance.GetPooledObject(projectilePrefab);

        projectile.GetComponent<DamageProjectile>().Launch(Stats, direction, launchForce, range, damage, targetTag);
    }
}