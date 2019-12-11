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
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float primaryAttackDelay = 0.6f;
    [Tooltip("How long until the player can attack after the primary attack")]
    public float primaryAttackCooldown;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip primaryAttackWeaponSFX;

    [Header("Secondary Attack")]
    [Tooltip("The maximum range the player's attack can reach")]
    public float secondaryAttackRadius = 6.8f;
    [Tooltip("The angular distance around the player where enemies are affected by the secondary attack")]
    public float secondaryAttackAngle = 60f;
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float secondaryAttackDelay = 0.6f;
    [Tooltip("How long until the player can attack after the secondary attack")]
    public float secondaryAttackCooldown;
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
        foreach (Transform enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), primaryAttackDelay));
        }
        
        // Primary attack animation
        //Anim.SetBool("Combo", !Anim.GetBool("Combo"));
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;
        
        AttackCooldown = secondaryAttackCooldown;
        StartCoroutine(CreateVFX(secondaryVFX, gameObject.transform.position, gameObject.transform.rotation, 
            PlayerManager.colours.GetColour(Stats.characterColour),  0.6f));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, secondaryAttackRadius, secondaryAttackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), secondaryAttackDelay));
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
