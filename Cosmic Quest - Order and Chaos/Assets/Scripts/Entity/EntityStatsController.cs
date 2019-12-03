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

public interface IEntityStatsController
{
    bool isDead { get; }

    float ComputeDamageModifer();
    float ComputeDefenseModifier();
    void TakeDamage(EntityStatsController attacker, float damageValue, float timeModifier = 1);
    void TakeExplosionDamage(EntityStatsController attacker, float maxDamage, float stunTime, float explosionForce, Vector3 explosionPoint, float explosionRadius);
}

public class EntityStatsController : MonoBehaviour, IEntityStatsController
{
    // Common entity regenerable stats
    public RegenerableStat health;

    public bool isDead { get; protected set; }

    // Common base stats
    public Stat damage;
    public Stat defense;

    public CharacterColour characterColour = CharacterColour.None;

    protected Animator Anim;
    protected Rigidbody rb;
    protected Collider col;

    // Entity layer mask constant for entity raycasting checks
    public const int EntityLayer = 1 << 9;

    protected virtual void Awake()
    {
        health.Init();

        Anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        if (!isDead)
            health.Regen();
    }

    public virtual void TakeDamage(EntityStatsController attacker, float damageValue, float timeModifier = 1f)
    {
        // Ignore attacks if already dead
        if (isDead)
            return;

        // Calculate any changes based on stats and modifiers here first
        float hitValue = (damageValue - ComputeDefenseModifier()) * timeModifier;
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
}
