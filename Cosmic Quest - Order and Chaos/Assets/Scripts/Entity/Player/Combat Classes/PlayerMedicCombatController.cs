using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMedicCombatController : PlayerCombatController
{
    [Header("Primary Attack - Wide Swipe")]
    [Tooltip("The minimum base damage that this attack can deal")]
    public float primaryAttackMinDamage = 0f;
    [Tooltip("The maximum base damage that this attack can deal")]
    public float primaryAttackMaxDamage = 5f;
    [Tooltip("The maximum range the player's melee attack can reach")]
    public float primaryAttackRadius = 4f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float primaryAttackSweepAngle = 100f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float primaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float primaryAttackManaDepletion = 25f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float primaryAttackMovementModifier = 0.5f;
    
    [Header("Secondary Attack - Healing Orb")]
    [Tooltip("The force to launch the healing projectile at")]
    public float secondaryAttackLaunchForce = 500f;
    [Tooltip("The range which the healing projectile can travel")]
    public float secondaryAttackRange = 20f;
    [Tooltip("The delay before the projectile is launched")]
    public float secondaryAttackLaunchDelay = 0.5f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float secondaryAttackManaDepletion = 30f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float secondaryAttackMovementModifier = 0.5f;
    [Tooltip("The prefab for the healing projectile")]
    public GameObject projectilePrefab;
    [Tooltip("How long until the player can attack after the secondary attack")]
    public float secondaryAttackCooldown;
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float secondaryAttackDelay = 0.6f;
    [Tooltip("Visual effect for secondary attack")]
    public GameObject secondaryVFX;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip secondaryAttackWeaponSFX;


    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < primaryAttackManaDepletion)
            return;

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackSweepAngle)))
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
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < secondaryAttackManaDepletion)
            return;

        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(projectilePrefab, transform.forward, secondaryAttackLaunchForce, secondaryAttackRange, secondaryAttackLaunchDelay));
        
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
