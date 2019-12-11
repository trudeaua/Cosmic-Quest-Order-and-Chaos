using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMedicCombatController : PlayerCombatController
{
    [Header("Primary Attack")]
    [Tooltip("The maximum range the player's melee attack can reach")]
    public float primaryAttackRadius = 2f;
    [Tooltip("The angular distance around the player where enemies are affected by the primary attack")]
    public float primaryAttackAngle = 100f;
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float primaryAttackDelay = 0.6f;
    [Tooltip("How long until the player can attack after the primary attack")]
    public float primaryAttackCooldown;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip primaryAttackWeaponSFX;

    [Header("Secondary Attack")]
    [Tooltip("The force to launch the healing projectile at")]
    public float secondaryAttackLaunchForce = 500f;
    [Tooltip("The range which the healing projectile can travel")]
    public float secondaryAttackRange = 20f;
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
        if (AttackCooldown > 0)
            return;

        AttackCooldown = primaryAttackCooldown;
        
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), primaryAttackDelay));
        }
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        
        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(projectilePrefab, transform.forward, secondaryAttackLaunchForce, secondaryAttackRange, secondaryAttackDelay));
    }

    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
        Anim.SetTrigger("UltimateAbility");

    }

    protected override void OnPrimaryAttack(InputValue value)
    {
        bool isPressed = value.isPressed;
        if (AttackCooldown <= 0)
        {
            if (isPressed)
            {
                StartCoroutine(Stats.PlayAudio(primaryAttackWeaponSFX));
                Anim.SetTrigger("PrimaryAttack");
                PrimaryAttack();
            }
        }
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        bool isPressed = value.isPressed;
        if (AttackCooldown <= 0)
        {
            if (isPressed)
            {
                StartCoroutine(Stats.PlayAudio(secondaryAttackWeaponSFX));
                Anim.SetTrigger("SecondaryAttack");
                SecondaryAttack();
            }
        }
    }
}
