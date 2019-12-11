using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeCombatController : PlayerCombatController
{
    [Header("Primary Attack - Fast Swing")]
    [Tooltip("The maximum range the player's attack can reach")]
    public float primaryAttackRadius = 2f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float primaryAttackSweepAngle = 45f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float primaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float primaryAttackManaDepletion = 25f;
    
    [Header("Secondary Attack - Wide Swing")]
    [Tooltip("The maximum range the player's attack can reach")]
    public float secondaryAttackRadius = 6.8f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float secondaryAttackSweepAngle = 60f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float secondaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float secondaryAttackManaDepletion = 50f;
    public GameObject secondaryVFX;

    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        // Ensure player has enough mana to perform this attack
        if ((Stats as PlayerStatsController).mana.CurrentValue < primaryAttackManaDepletion)
            return;

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);

        // Attack any enemies within the attack sweep and range
        foreach (Transform enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackSweepAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), primaryAttackDamageDelay));
        }
        
        // Reset attack timeout and deplete mana
        AttackCooldown = primaryAttackTimeout;
        (Stats as PlayerStatsController).mana.Subtract(primaryAttackManaDepletion);

        // Primary attack animation
        //Anim.SetBool("Combo", !Anim.GetBool("Combo"));
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;
        
        // Ensure player has enough mana to perform this attack
        if ((Stats as PlayerStatsController).mana.CurrentValue < secondaryAttackManaDepletion)
            return;

        StartCoroutine(CreateVFX(secondaryVFX, gameObject.transform.position, gameObject.transform.rotation, 
            PlayerManager.colours.GetColour(Stats.characterColour), 0.6f));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, secondaryAttackRadius, secondaryAttackSweepAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), secondaryAttackDamageDelay));
        }
        
        // Reset attack timeout and deplete mana
        AttackCooldown = secondaryAttackTimeout;
        (Stats as PlayerStatsController).mana.Subtract(secondaryAttackManaDepletion);
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
                // Play full animation then set the bool to false
                StartCoroutine(FinishPrimaryAttack());
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
                Anim.SetTrigger("SecondaryAttack");
                SecondaryAttack();
            }
        }
    }

    private IEnumerator FinishPrimaryAttack()
    {
        yield return new WaitForSeconds(primaryAttackTimeout);
        Anim.SetBool("PrimaryAttack", false);
    }
}
