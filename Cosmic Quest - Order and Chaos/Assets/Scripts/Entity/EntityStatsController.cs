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
    // Common entity regenerable stats
    public RegenerableStat health;

    public bool isDead { get; protected set; }

    // Common base stats
    public Stat baseDamage;
    public Stat baseDefense;

    public CharacterColour characterColour = CharacterColour.None;

    private void Awake()
    {
        health = new RegenerableStat(100, 0, 0, 0f);

        // Add combat class stat modifiers to base stats
        //baseDamage.AddModifier(combatClass.baseDamageModifier);
        //baseDefense.AddModifier(combatClass.baseDefenseModifier);
    }

    public virtual void TakeDamage(EntityStatsController attacker, int damage)
    {
        // Calculate any changes based on stats and modifiers here first
        health.Subtract(damage);

        if (health.currentValue == 0)
        {
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
