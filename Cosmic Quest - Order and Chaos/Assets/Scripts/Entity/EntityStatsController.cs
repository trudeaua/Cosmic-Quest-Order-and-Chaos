using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterColour
{
    None,
    All,
    Red,
    Yellow,
    Green,
    Blue
}

public class EntityStatsController : MonoBehaviour
{
    // Common entity regenerable stats
    public RegenerableStat health;

    public bool isDead { get; protected set; }

    public UnityEvent onDeath = new UnityEvent();

    // Common base stats
    public Stat damage;
    public Stat defense;

    public CharacterColour characterColour = CharacterColour.None;

    protected Animator Anim;
    protected Rigidbody rb;
    protected Collider col;

    // Entity layer mask constant for entity raycasting checks
    public const int EntityLayer = 1 << 9;

    [Header("Spawn Config")]
    [Tooltip("Indicates whether the spawn animation should play or not")]
    [SerializeField] protected bool shouldSpawn = true;
    [Tooltip("VFX to use when player spawns")]
    [SerializeField] protected GameObject spawnVFX;
    [Tooltip("Controls the speed of the spawn animation")]
    [SerializeField] protected float spawnSpeed = 0.08f;
    [Tooltip("Number of seconds to wait before the spawn sequence starts")]
    [SerializeField] protected float spawnDelay = 0.9f;
    [Tooltip("Number of seconds to wait after the spawn sequence ends")]
    [SerializeField] protected float spawnCooldown = 0;

    protected AudioSource[] Audio;
    // Audio source for playing vocal audio clips
    protected AudioSource VocalAudio;
    // Audio source for playing weapon audio clips
    protected AudioSource WeaponAudio;
    [Header("Audio Clips")]
    [Tooltip("Audio clip that should play when the entity takes damage")]
    [SerializeField] protected AudioHelper.EntityAudioClip takeDamageVocalSFX;
    [Tooltip("Audio clip that should play when the entity dies")]
    [SerializeField] protected AudioHelper.EntityAudioClip entityDeathVocalSFX;

    protected virtual void Awake()
    {
        health.Init();

        Anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

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
        if (isDead)
            health.PauseRegen();
    }

    public void SetSpawn(bool spawn)
    {
        shouldSpawn = spawn;
    }

    /// <summary>
    /// Take damage from an attacker
    /// </summary>
    /// <param name="attacker">Stats controller of the attacking entity</param>
    /// <param name="damageValue">Approximate damage value to apply to enemy health</param>
    /// <param name="timeDelta">Time since last damage calculation</param>
    public virtual void TakeDamage(EntityStatsController attacker, float damageValue, float timeDelta = 1f)
    {
        // Ignore attacks if already dead
        if (isDead)
            return;
        Anim.ResetTrigger("TakeDamage");
        Anim.SetTrigger("TakeDamage");
        if (takeDamageVocalSFX != null)
        {
            StartCoroutine(AudioHelper.PlayAudioOverlap(VocalAudio, takeDamageVocalSFX));
        }
        // Calculate any changes based on stats and modifiers here first
        float hitValue = (damageValue - ComputeDefenseModifier()) * timeDelta;
        health.Subtract(hitValue < 0 ? 0 : hitValue);

        if (Mathf.Approximately(health.CurrentValue, 0f))
        {
            Die();
        }
    }

    /// <summary>
    /// Take damage from an attacker
    /// </summary>
    /// <param name="attacker">Stats controller of the attacking entity</param>
    /// <param name="maxDamage">Maximum amount of damage able to be dealt</param>
    /// <param name="stunTime">Amount of time to stun the enemy</param>
    /// <param name="explosionForce">Value to display</param>
    /// <param name="explosionPoint">Where the explosion originates from</param>
    /// <param name="explosionRadius">Explosion effect radius</param>
    public virtual void TakeExplosionDamage(EntityStatsController attacker, float maxDamage, float stunTime,
        float explosionForce, Vector3 explosionPoint, float explosionRadius)
    {
        // Ignore explosions if already dead
        if (isDead)
            return;

        // Calculate damage based on distance from the explosion point
        float proximity = (col.ClosestPoint(explosionPoint) - explosionPoint).magnitude;
        float effect = 1 - (proximity / explosionRadius);

        // TODO slightly strange bug where enemies just beyond the explosion take negative damage? This is a temp fix.
        if (effect < 0f)
            return;

        TakeDamage(attacker, maxDamage * effect);

        StartCoroutine(ApplyExplosiveForce(explosionForce, explosionPoint, explosionRadius, stunTime));
    }

    /// <summary>
    /// Apply an explosive force to the rigidbody
    /// </summary>
    /// <param name="explosionForce">Value to display</param>
    /// <param name="explosionPoint">Where the explosion originates from</param>
    /// <param name="explosionRadius">Explosion effect radius</param>
    /// <param name="stunTime">Amount of time to stun the enemy</param>
    /// <returns>An IEnumerator</returns>
    protected virtual IEnumerator ApplyExplosiveForce(float explosionForce, Vector3 explosionPoint, float explosionRadius, float stunTime)
    {
        // Set to stunned before applying explosive force
        // TODO

        // TODO change this to AddForce(<force vector>, ForceMode.Impulse);
        rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);

        // Wait for a moment before un-stunning the victim
        yield return new WaitForSeconds(stunTime);
    }

    /// <summary>
    /// Compute the entity's damage modifier
    /// </summary>
    /// <returns>The entity's damage modifier</returns>
    public virtual float ComputeDamageModifer()
    {
        float baseHit = Random.Range(0, damage.GetBaseValue() - 1); // never want to do 0 damage
        return damage.GetValue() - baseHit;
    }
    /// <summary>
    /// Compute the amount of defense to take
    /// </summary>
    /// <returns>The entity's defense modifier</returns>
    public virtual float ComputeDefenseModifier()
    {
        float baseDefense = Random.Range(0, defense.GetBaseValue());
        return defense.GetValue() - baseDefense;
    }

    /// <summary>
    /// Kill the entity
    /// </summary>
    protected virtual void Die()
    {
        // Meant to be implemented with any death tasks
        isDead = true;
    }

    /// <summary>
    /// "Spawn" the entity by causing them to float up through the stage
    /// </summary>
    /// <param name="obj">Object to spawn</param>
    /// <param name="speed">How fast the spawn should be</param>
    /// <param name="delay">How many seconds to wait before spawning</param>
    /// <param name="cooldown">How many seconds to wait before enabling the enemy's movement</param>
    protected virtual IEnumerator Spawn(GameObject obj, float speed = 0.05f, float delay = 0f, float cooldown = 0f)
    {
        Collider col = obj.GetComponent<Collider>();
        float from = -1 * col.bounds.size.y * 2.5f;
        float to = obj.transform.position.y;
        col.enabled = false;
        obj.transform.position = new Vector3(obj.transform.position.x, from, obj.transform.position.z);
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        // apply any spawn animation
        foreach (AnimatorControllerParameter parameter in Anim.parameters)
        {
            if (parameter.name == "Spawn")
            {
                Anim.SetTrigger("Spawn");
                break;
            }
        }
        float offset = 0;
        while (obj.transform.position.y < to)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, from + offset, obj.transform.position.z);
            offset += speed;
            yield return new WaitForSeconds(0.01f);
        }
        col.enabled = true;
        if (cooldown > 0)
        {
            yield return new WaitForSeconds(cooldown);
        }
    }
}