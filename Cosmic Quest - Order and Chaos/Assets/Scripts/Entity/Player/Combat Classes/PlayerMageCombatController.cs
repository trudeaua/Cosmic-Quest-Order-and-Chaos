using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMageCombatController : PlayerCombatController
{
    [Tooltip("The distance the primary projectile attack can travel")]
    public float primaryAttackRange = 20f;
    [Tooltip("The force to launch the primary attack projectile at")]
    public float primaryAttackLaunchForce = 500f;
    [Tooltip("The projectile prefab for the primary attack")]
    public GameObject primaryProjectilePrefab;
    [Tooltip("The distance the secondary attack will reach")]
    public float secondaryAttackRange = 3f;
    [Tooltip("The secondary attack projected angle of AOE in degrees")]
    public float secondaryAttackAngle = 60f;
    [Tooltip("The damage per second of the secondary attack")]
    public float secondaryAttackDps = 5f;
    
    private bool _isSecondaryActive = false;

    protected override void Update()
    {
        base.Update();

        if (_isSecondaryActive)
        {
            SecondaryAttack();
        }
    }

    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = primaryAttackCooldown;
        
        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(primaryProjectilePrefab, transform.forward, primaryAttackLaunchForce, primaryAttackRange, 0.5f));
    }
    
    protected override void SecondaryAttack()
    {
        // TODO this may be an inefficient way to do this...
        
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRange);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy.position, secondaryAttackRange, secondaryAttackAngle)))
        {
            // Calculate and perform damage at DPS rate
            enemy.GetComponent<EntityStatsController>().TakeDamage(Stats, secondaryAttackDps * Time.deltaTime);
        }
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        // Ensure secondary is only activated on button down
        _isSecondaryActive = value.isPressed;
    }
}
