using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMageCombatController : PlayerCombatController
{
    [Header("Primary Attack - Flamethrower Spell")]
    [Tooltip("The distance the primary attack will reach")]
    public float primaryAttackRadius = 3f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float primaryAttackSweepAngle = 60f;
    [Tooltip("The amount of mana depleted per second")]
    public float primaryAttackManaDepletion = 20f;
    [Tooltip("Visual effect for primary attack")]
    public GameObject primaryVFX;

    [Header("Secondary Attack - AOE Explosion")]
    [Tooltip("The radius of the AOE effect")]
    public float secondaryAttackRadius = 8f;
    [Tooltip("The explosive force of the AOE effect")]
    public float secondaryAttackExplosionForce = 500f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float secondaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float secondaryAttackManaDepletion = 20f;
    [Tooltip("Visual effect for secondary attack")]
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
        // Ensure player has enough mana to perform this attack
        if ((Stats as PlayerStatsController).mana.CurrentValue < 1f)
        {
            // Stop attack if not enough mana
            _isPrimaryActive = false;
            Anim.SetBool("PrimaryAttack", false);
            (Stats as PlayerStatsController).mana.StartRegen();
            return;
        }

        (Stats as PlayerStatsController).mana.Subtract(primaryAttackManaDepletion * Time.deltaTime);
        
        Vector3 vfxPos = gameObject.transform.position + gameObject.transform.forward * 1.5f + new Vector3(0, 2f);
        StartCoroutine(CreateVFX(primaryVFX, vfxPos, gameObject.transform.rotation));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackSweepAngle)))
        {
            // TODO this calculation needs to be changed - currently uses the damage amount as the DPS
            float baseDamage = Stats.damage.GetValue();
            enemy.GetComponent<EntityStatsController>().TakeDamage(Stats, baseDamage, Time.deltaTime);
        }
    }
    
    protected override void SecondaryAttack()
    {
        // Ensure player has enough mana to perform this attack
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < secondaryAttackManaDepletion)
            return;
        
        Anim.SetTrigger("SecondaryAttack");
        
        StartCoroutine(CreateVFX(secondaryVFX, gameObject.transform.position, Quaternion.identity, PlayerManager.colours.GetColour(Stats.characterColour)));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the AOE range
        foreach (var enemy in enemies)
        {
            StartCoroutine(PerformExplosiveDamage(enemy.GetComponent<EntityStatsController>(), 
                Stats.damage.GetValue(), 2f, secondaryAttackExplosionForce, transform.position, secondaryAttackRadius, secondaryAttackDamageDelay));
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
        if (value.isPressed)
        {
            _isPrimaryActive = true;
            Anim.SetBool("PrimaryAttack", true);
            (Stats as PlayerStatsController).mana.PauseRegen();
        }
        else
        {
            _isPrimaryActive = false;
            Anim.SetBool("PrimaryAttack", false);
            (Stats as PlayerStatsController).mana.StartRegen();
        }
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        if (!_isPrimaryActive && value.isPressed)
        {
            SecondaryAttack();
        }
    }
}
