﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMeleeCombatController : PlayerCombatController
{
    [Header("Primary Attack - Fast Swing")]
    [Tooltip("The minimum base damage that this attack can deal")]
    public float primaryAttackMinDamage = 0f;
    [Tooltip("The maximum base damage that this attack can deal")]
    public float primaryAttackMaxDamage = 5f;
    [Tooltip("The maximum range the player's attack can reach")]
    public float primaryAttackRadius = 3f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float primaryAttackSweepAngle = 45f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float primaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float primaryAttackManaDepletion = 25f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float primaryAttackMovementModifier = 0.5f;
    
    [Header("Secondary Attack - Wide Swing")]
    [Tooltip("The minimum base damage that this attack can deal")]
    public float secondaryAttackMinDamage = 2f;
    [Tooltip("The maximum base damage that this attack can deal")]
    public float secondaryAttackMaxDamage = 6f;
    [Tooltip("The maximum range the player's attack can reach")]
    public float secondaryAttackRadius = 6.8f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float secondaryAttackSweepAngle = 60f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float secondaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float secondaryAttackManaDepletion = 50f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float secondaryAttackMovementModifier = 0.5f;
    [Tooltip("Visual effect for secondary attack")]
    public GameObject secondaryVFX;

    protected override void PrimaryAttack()
    {
        // Ensure player has enough mana to perform this attack
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < primaryAttackManaDepletion)
            return;

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);

        // Attack any enemies within the attack sweep and range
        foreach (Transform enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackSweepAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            float damageValue = Random.Range(primaryAttackMinDamage, primaryAttackMaxDamage + Stats.damage.GetValue());
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), damageValue, primaryAttackDamageDelay));
        }
        
        // Trigger primary attack animation
        StartCoroutine(TriggerTimeAttackAnimation("PrimaryAttack", primaryAttackTimeout));
        
        // Reset attack timeout and deplete mana
        AttackCooldown = primaryAttackTimeout;
        (Stats as PlayerStatsController).mana.Subtract(primaryAttackManaDepletion);
        
        // Apply movement speed modifier
        StartCoroutine(Motor.ApplyTimedMovementModifier(primaryAttackMovementModifier, primaryAttackTimeout));

        // Primary attack animation
        //Anim.SetBool("Combo", !Anim.GetBool("Combo"));
    }
    
    protected override void SecondaryAttack()
    {
        // Ensure player has enough mana to perform this attack
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < secondaryAttackManaDepletion)
            return;

        StartCoroutine(VfxHelper.CreateVFX(secondaryVFX, transform.position, transform.rotation, 
            PlayerManager.colours.GetColour(Stats.characterColour), secondaryAttackDamageDelay));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, secondaryAttackRadius, secondaryAttackSweepAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(secondaryAttackMinDamage, secondaryAttackMaxDamage + Stats.damage.GetValue());
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), damageValue, secondaryAttackDamageDelay));
        }
        
        // Trigger secondary attack animation
        Anim.SetTrigger("SecondaryAttack");
        
        // Reset attack timeout and deplete mana
        AttackCooldown = secondaryAttackTimeout;
        (Stats as PlayerStatsController).mana.Subtract(secondaryAttackManaDepletion);
        
        // Apply movement speed modifier
        StartCoroutine(Motor.ApplyTimedMovementModifier(secondaryAttackMovementModifier, secondaryAttackTimeout));
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
        Anim.SetTrigger("UltimateAbility");
    }
}
