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

    protected IEnumerator PerformDamage(EntityStatsController targetStats, float damageValue, float damageDelay = 0f)
    {
        if (damageDelay > 0f)
            yield return new WaitForSeconds(damageDelay);

        ResetTakeDamageAnim();

        // Applies damage to targetStats
        targetStats.TakeDamage(Stats, damageValue);
    }

    protected IEnumerator PerformExplosiveDamage(EntityStatsController targetStats, float maxDamage, float stunTime,
        float explosionForce, Vector3 explosionPoint, float explosionRadius, float explosionDelay = 0f)
    {
        if (explosionDelay > 0f)
            yield return new WaitForSeconds(explosionDelay);

        ResetTakeDamageAnim();

        // Applies damage to targetStats
        targetStats.TakeExplosionDamage(Stats, maxDamage, stunTime, explosionForce, explosionPoint, explosionRadius);
    }

    protected void ResetTakeDamageAnim()
    {
        Anim.ResetTrigger("TakeDamage");
    }

    protected IEnumerator LaunchProjectile(GameObject projectilePrefab, Vector3 direction, float launchForce, float range, float launchDelay = 0f)
    {
        if (launchDelay > 0f)
            yield return new WaitForSeconds(launchDelay);

        // Launch projectile from projectile pool
        GameObject projectile = ObjectPooler.Instance.GetPooledObject(projectilePrefab);

        ResetTakeDamageAnim();
        projectile.GetComponent<Projectile>().Launch(Stats, direction, launchForce, range);
    }
}