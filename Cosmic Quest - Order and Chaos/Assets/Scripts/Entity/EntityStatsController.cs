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
    protected Rigidbody rb;
    
    // Entity layer mask constant for entity raycasting checks
    public const int EntityLayer = 1 << 9;
    
    protected virtual void Awake()
    {
        health.Init();

        Anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
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
        if (isDead)
            return;
        
        // Calculate damage based on distance from the explosion point
        float proximity = (transform.position - explosionPoint).magnitude;
        float effect = 1 - (proximity / explosionRadius);
        TakeDamage(attacker, maxDamage * effect);
        rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
    }
    
    public virtual float ComputeDamageModifer()
    {
        float baseHit = Random.Range(0, damage.GetBaseValue());
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
