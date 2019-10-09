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

public class EntityStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; protected set; }
    //public CombatClass combatClass { get; private set; }

    public bool isDead { get; protected set; } = false;

    // public Stat[] baseStats;
    public Stat baseDamage;
    public Stat baseDefense;

    public CharacterColour characterColour = CharacterColour.None;

    void Awake()
    {
        // Start off with full health
        currentHealth = maxHealth;

        // Add combat class stat modifiers to base stats
        //baseDamage.AddModifier(combatClass.baseDamageModifier);
        //baseDefense.AddModifier(combatClass.baseDefenseModifier);

        // TODO this is for testing only!
        /*Renderer renderer = GetComponentInChildren<Renderer>();
        switch (characterColour)
        {
            case CharacterColour.Red:
                renderer.material.SetColor("_Color", Color.red);
                break;
            case CharacterColour.Green:
                renderer.material.SetColor("_Color", Color.green);
                break;
            case CharacterColour.Blue:
                renderer.material.SetColor("_Color", Color.blue);
                break;
            case CharacterColour.Purple:
                renderer.material.SetColor("_Color", new Color(1f, 0f, 1f));
                break;
        }*/
    }

    public virtual void TakeDamage(EntityStats attacker, int damage)
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
        isDead = true;
        Debug.Log(transform.name + " died.");
    }
}
