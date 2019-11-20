using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMageCombatController : PlayerCombatController
{
    [Header("Primary Attack")]
    [Tooltip("The distance the secondary attack will reach")]
    public float primaryAttackRange = 3f;
    [Tooltip("The secondary attack projected angle of AOE in degrees")]
    public float primaryAttackAngle = 60f;
    [Tooltip("The damage per second of the secondary attack")]
    public float primaryAttackDps = 5f;

    [Header("Secondary Attack")]
    [Tooltip("The radius of the AOE effect")]
    public float secondaryAttackRadius = 8f;
    [Tooltip("The explosive force of the AOE effect")]
    public float secondaryAttackForce = 500f;

    private bool _isPrimaryActive = false;

    protected override void Update()
    {
        base.Update();

        if (_isPrimaryActive)
        {
            PrimaryAttack();
        }
    }

    protected override void PrimaryAttack()
    {
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRange);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRange, primaryAttackAngle)))
        {
            // Calculate and perform damage at DPS rate
            enemy.GetComponent<EntityStatsController>().TakeDamage(Stats, primaryAttackDps, Time.deltaTime);
        }
        
        // Cast spell animation
        Anim.SetTrigger("PrimaryAttack");
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the AOE range
        foreach (var enemy in enemies)
        {
            StartCoroutine(PerformExplosiveDamage(enemy.GetComponent<EntityStatsController>(), 
                Stats.damage.GetValue(), 2f, secondaryAttackForce, transform.position, secondaryAttackRadius, 0.6f));
        }
        
        Anim.SetTrigger("SecondaryAttack");
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
    }

    protected override void OnPrimaryAttack(InputValue value)
    {
        // Ensure secondary is only activated on button down
        _isPrimaryActive = value.isPressed;
    }
}
