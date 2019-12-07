using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMeleeCombatController : PlayerCombatController
{
    [Header("Primary Attack")]
    [Tooltip("The maximum range the player's attack can reach")]
    public float primaryAttackRadius = 2f;
    [Tooltip("The angular distance around the player where enemies are affected by the primary attack")]
    public float primaryAttackAngle = 45f;
    
    [Header("Secondary Attack")]
    [Tooltip("The angular distance around the player where enemies are affected by the secondary attack")]
    public float secondaryAttackAngle = 160f;
    public GameObject secondaryVFX;
    
    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = primaryAttackCooldown;
        
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);

        // Attack any enemies within the attack sweep and range
        foreach (Transform enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), 0.6f));
        }
        
        // Primary attack animation
        //Anim.SetBool("Combo", !Anim.GetBool("Combo"));
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;
        
        AttackCooldown = secondaryAttackCooldown;
        StartCoroutine(CreateVFX(secondaryVFX, gameObject.transform, gameObject.transform.rotation, 0.6f));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, secondaryAttackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), 0.6f));
        }
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
            Anim.SetBool("PrimaryAttack", isPressed);
            if (isPressed)
            {
                PrimaryAttack();
            }
        }
        else
        {
            Anim.SetBool("PrimaryAttack", false);
        }
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        bool isPressed = value.isPressed;
        if (AttackCooldown <= 0)
        {
            if (isPressed)
            {
                Anim.SetTrigger("SecondaryAttack");
                SecondaryAttack();
            }
        }
    }
}
