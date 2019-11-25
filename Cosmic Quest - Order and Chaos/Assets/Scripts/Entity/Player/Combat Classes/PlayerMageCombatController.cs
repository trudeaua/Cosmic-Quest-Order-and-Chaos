using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMageCombatController : PlayerCombatController
{
    [Header("Primary Attack")]
    [Tooltip("The distance the primary attack will reach")]
    public float primaryAttackRange = 3f;
    [Tooltip("The primary attack projected angle of AOE in degrees")]
    public float primaryAttackAngle = 60f;
    [Tooltip("The damage per second of the primary attack")]
    public float primaryAttackDps = 5f;

    [Header("Secondary Attack")]
    [Tooltip("The radius of the AOE effect")]
    public float secondaryAttackRadius = 8f;
    [Tooltip("The explosive force of the AOE effect")]
    public float secondaryAttackForce = 500f;
    public GameObject secondaryVFX;

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
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        StartCoroutine(CreateVFX(secondaryVFX, gameObject.transform, Quaternion.identity));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the AOE range
        foreach (var enemy in enemies)
        {
            StartCoroutine(PerformExplosiveDamage(enemy.GetComponent<EntityStatsController>(), 
                Stats.damage.GetValue(), 2f, secondaryAttackForce, transform.position, secondaryAttackRadius, 0.6f));
        }
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
        Anim.SetTrigger("UltimateAbility");
    }

    protected override void OnPrimaryAttack(InputValue value)
    {
        _isPrimaryActive = value.isPressed;
        Anim.SetBool("PrimaryAttack", _isPrimaryActive);
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        bool isPressed = value.isPressed;
        Anim.SetBool("SecondaryAttack", isPressed);
        if (isPressed)
        {
            SecondaryAttack();
        }
    }
}
