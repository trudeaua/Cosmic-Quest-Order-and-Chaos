using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    //public CombatClass combatClass { get; private set; }

    // public Stat[] baseStats;
    public Stat baseDamage;
    public Stat baseDefense;

    void Awake()
    {
        // Start off with full health
        currentHealth = maxHealth;

        // Add combat class stat modifiers to base stats
        //baseDamage.AddModifier(combatClass.baseDamageModifier);
        //baseDefense.AddModifier(combatClass.baseDefenseModifier);
    }

    public void TakeDamage(int damage)
    {
        // Calculate any changes based on stats and modifiers here first
        currentHealth -= damage;
        Debug.Log(transform.name + " took " + damage + " damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        // Meant to be implemented with any death tasks
        Debug.Log(transform.name + " died.");
    }
}
