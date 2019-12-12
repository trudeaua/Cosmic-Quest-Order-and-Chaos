using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterColour
{
    None,
    Red,
    Yellow,
    Green,
    Purple
}

// Audio clip to play along with some parameters
[System.Serializable]
public class EntityAudioClip {
    public enum AudioType { 
        Weapon,
        Vocal
    }
    [Tooltip("Pitch of the audio (affects tempo as well)")]
    public float pitch;
    [Tooltip("How loud the audio should be")]
    public float volume;
    [Tooltip("Audio clip file")]
    public AudioClip clip;
    [Tooltip("Is the audio vocal or from a weapon?")]
    public AudioType type;
    [Tooltip("Time in seconds until audio plays")]
    public float delay;
    [Tooltip("Should the audio loop?")]
    public bool loop;

}

public class EntityStatsController : MonoBehaviour
{
    // Common entity regenerable stats
    public RegenerableStat health;

    public bool isDead { get; protected set; }

    // Common base stats
    public Stat damage;
    public Stat defense;

    public CharacterColour characterColour = CharacterColour.None;

    protected Animator Anim;
    protected AudioSource[] Audio;
    // Audio source for playing vocal audio clips
    protected AudioSource VocalAudio;
    // Audio source for playing weapon audio clips
    protected AudioSource WeaponAudio;
    protected Rigidbody rb;
    protected Collider col;
    [SerializeField] protected EntityAudioClip takeDamageVocalSFX;
    [SerializeField] protected EntityAudioClip entityDeathVocalSFX;

    // Entity layer mask constant for entity raycasting checks
    public const int EntityLayer = 1 << 9;

    protected virtual void Awake()
    {
        health.Init();

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
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        if (!isDead)
            health.Regen();
    }

    public virtual void TakeDamage(EntityStatsController attacker, float damageValue)
    {
        // Ignore attacks if already dead
        if (isDead)
            return;
        Anim.ResetTrigger("TakeDamage");
        Anim.SetTrigger("TakeDamage");
        if (takeDamageVocalSFX != null)
        {
            StartCoroutine(PlayAudioOverlap(takeDamageVocalSFX));
        }
        // Calculate any changes based on stats and modifiers here first
        float hitValue = damageValue - ComputeDefenseModifier();
        health.Subtract(hitValue < 0 ? 0 : hitValue);

        if (Mathf.Approximately(health.CurrentValue, 0f))
        {
            Die();
        }
    }

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

    protected virtual IEnumerator ApplyExplosiveForce(float explosionForce, Vector3 explosionPoint, float explosionRadius, float stunTime)
    {
        // Set to stunned before applying explosive force
        // TODO

        // TODO change this to AddForce(<force vector>, ForceMode.Impulse);
        rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);

        // Wait for a moment before un-stunning the victim
        yield return new WaitForSeconds(stunTime);
    }

    public virtual float ComputeDamageModifer()
    {
        float baseHit = Random.Range(0, damage.GetBaseValue() - 1); // never want to do 0 damage
        return damage.GetValue() - baseHit;
    }

    public virtual float ComputeDefenseModifier()
    {
        float baseDefense = Random.Range(0, defense.GetBaseValue());
        return defense.GetValue() - baseDefense;
    }

    protected virtual void Die()
    {
        // Meant to be implemented with any death tasks
        isDead = true;
    }

    /// <summary>
    /// Plays an audio clip that overlaps with currently playing audio clips from one of the entity's audio sources
    /// </summary>
    /// <param name="entityAudio">Entity audio clip to play</param>
    /// <returns>An IEnumerator</returns>
    public virtual IEnumerator PlayAudioOverlap(EntityAudioClip entityAudio)
    {
        AudioSource audio = entityAudio.type == EntityAudioClip.AudioType.Vocal ? VocalAudio : WeaponAudio;
        if (entityAudio.delay > 0)
        {
            yield return new WaitForSeconds(entityAudio.delay);
        }
        audio.pitch = entityAudio.pitch;
        audio.volume = entityAudio.volume;
        audio.loop = entityAudio.loop;
        audio.PlayOneShot(entityAudio.clip);
    }

    /// <summary>
    /// Plays an audio clip that interrupts the currently playing audio clip(s) from one of the entity's audio sources
    /// </summary>
    /// <param name="entityAudio">Entity audio clip to play</param>
    /// <returns>An IEnumerator</returns>
    public virtual IEnumerator PlayAudio(EntityAudioClip entityAudio)
    {
        AudioSource audio = entityAudio.type == EntityAudioClip.AudioType.Vocal ? VocalAudio : WeaponAudio;
        if (entityAudio.delay > 0)
        {
            yield return new WaitForSeconds(entityAudio.delay);
        }
        audio.pitch = entityAudio.pitch;
        audio.volume = entityAudio.volume;
        audio.loop = entityAudio.loop;
        audio.clip = entityAudio.clip;
        audio.Play();
    }

    /// <summary>
    /// Stops the given entity audio clip from playing
    /// </summary>
    /// <param name="entityAudio">Entity audio clip to stop playing</param>
    public virtual void StopAudio(EntityAudioClip entityAudio)
    {
        AudioSource audio = entityAudio.type == EntityAudioClip.AudioType.Vocal ? VocalAudio : WeaponAudio;
        audio.Stop();
    }
}
