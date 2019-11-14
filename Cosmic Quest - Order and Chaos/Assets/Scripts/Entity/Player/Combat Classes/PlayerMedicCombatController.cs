using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMedicCombatController : PlayerCombatController
{
    [Tooltip("The maximum range the player's melee attack can reach")]
    public float meleeAttackRadius = 2f;
    
    [Tooltip("The angular distance around the player where enemies are affected by the primary attack")]
    public float primaryAttackAngle = 100f;

    [Tooltip("The velocity at which the healing projectile will travel")]
    public float projectileVelocity = 10f;

    [Tooltip("The range which the healing projectile can travel")]
    public float projectileRange = 20f;
    
    [Tooltip("The prefab for the healing projectile")]
    public GameObject projectilePrefab;
    
    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = primaryAttackCooldown;
        
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(meleeAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy.position, meleeAttackRadius, primaryAttackAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), 0.6f));
        }
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        
        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(projectilePrefab, transform.forward, projectileVelocity, projectileRange, 0.5f));
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
    }
}
