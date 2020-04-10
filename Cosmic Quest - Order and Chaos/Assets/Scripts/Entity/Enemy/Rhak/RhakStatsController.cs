using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RhakStatsController : EnemyStatsController
{
    // List of orbs required to be hit by for phase 1
    public List<CharacterColour> OrbsRequired { get; private set; }

    private void Start()
    {
        // Initialize the orb colours to all the player colours
        OrbsRequired = PlayerManager.Instance.CurrentPlayerColours.ToList();
    }
    
    /// <summary>
    /// Take damage from an attacker
    /// </summary>
    /// <param name="attacker">Stats controller of the attacking entity</param>
    /// <param name="damageValue">Approximate damage value to apply to enemy health</param>
    /// <param name="timeDelta">Time since last damage calculation</param>
    public override void TakeDamage(EntityStatsController attacker, float damageValue, float timeDelta = 1f)
    {
        // Ignore attacks if already dead or invincible
        if (isDead || invincible)
            return;

        float colourDamagePercentage = characterColour == CharacterColour.All || attacker.characterColour == characterColour ? 1 : colourResistanceModifier;

        // Calculate any changes based on stats and modifiers here first
        float hitValue = Mathf.Max(colourDamagePercentage * (damageValue - ComputeDefenseModifier()), 0) * timeDelta;
        health.Subtract(hitValue);
        ShowDamage(hitValue);

        // Pass damage information to brain
        Brain.OnDamageTaken(attacker.gameObject, hitValue);

        if (Mathf.Approximately(health.CurrentValue, 0f))
        {
            Die();
        }
    }

    /// <summary>
    /// Take damage from a coloured orb in the final boss fight
    /// </summary>
    /// <param name="orbColour">The colour of the orb which is dealing damage</param>
    public void TakeDamageFromOrb(CharacterColour orbColour)
    {
        if (!OrbsRequired.Contains(orbColour))
            return;

        // Take enough damage such that the boss will be at half health for phase 2
        float hitValue = (1f / PlayerManager.Instance.NumPlayers) * 0.5f * health.maxValue;
        health.Subtract(hitValue);
        ShowDamage(hitValue);
        
        // Remove the orb colour from the list of colours still required
        OrbsRequired.Remove(orbColour);
    }
    
    /// <summary>
    /// Handles death activities for Rhak
    /// </summary>
    protected override void Die()
    {
        isDead = true;
        Anim.SetTrigger("Die");
    }

    /// <summary>
    /// Event function for Rhak's death
    /// </summary>
    public void RhakDeath()
    {
        onDeath.Invoke();
        StartCoroutine(AudioHelper.PlayAudioOverlap(VocalAudio, entityDeathVocalSFX));
    }
}