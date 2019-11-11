using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterColour
{
    None,
    Red,
    Blue,
    Green,
    Purple
}

public class EntityStatsController : MonoBehaviour
{
    // Common entity health stats
    public float maxHealth = 100f;
    public float currentHealth { get; protected set; }

    public bool isDead { get; protected set; }

    // Common base stats
    public Stat baseDamage;
    public Stat baseDefense;

    public CharacterColour characterColour = CharacterColour.None;

    private void Awake()
    {
        // Start off with full health
        currentHealth = maxHealth;

        // Add combat class stat modifiers to base stats
        //baseDamage.AddModifier(combatClass.baseDamageModifier);
        //baseDefense.AddModifier(combatClass.baseDefenseModifier);
    }

    public virtual void TakeDamage(EntityStatsController attacker, int damage)
    {
        // Calculate any changes based on stats and modifiers here first
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        // Meant to be implemented with any death tasks
        isDead = true;
        Debug.Log(transform.name + " died.");
    }
}
