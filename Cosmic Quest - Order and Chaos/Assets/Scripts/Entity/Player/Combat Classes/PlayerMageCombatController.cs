using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMageCombatController : PlayerCombatController
{
    [Header("Primary Attack - Flamethrower Spell")]
    [Tooltip("The minimum base damage that this attack can deal")]
    public float primaryAttackMinDamage = 0f;
    [Tooltip("The maximum base damage that this attack can deal")]
    public float primaryAttackMaxDamage = 5f;
    [Tooltip("The distance the primary attack will reach")]
    public float primaryAttackRadius = 3f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float primaryAttackSweepAngle = 60f;
    [Tooltip("The amount of mana depleted per second")]
    public float primaryAttackManaDepletion = 20f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float primaryAttackMovementModifier = 0.5f;
    [Tooltip("Visual effect for primary attack")]
    public GameObject primaryVFX;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackWeaponSFX;

    [Header("Secondary Attack - AOE Explosion")]
    [Tooltip("The maximum base damage that this attack can deal")]
    public float secondaryAttackMaxDamage = 6f;
    [Tooltip("The time in seconds that affected enemies are stunned")]
    public float secondaryAttackStunTime = 1.5f;
    [Tooltip("The radius of the AOE effect")]
    public float secondaryAttackRadius = 8f;
    [Tooltip("The explosive force of the AOE effect")]
    public float secondaryAttackExplosionForce = 500f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float secondaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float secondaryAttackManaDepletion = 50f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float secondaryAttackMovementModifier = 0.5f;
    [Tooltip("Visual effect for secondary attack")]
    public GameObject secondaryVFX;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackWeaponSFX;

    private bool _isPrimaryActive = false;

    protected override void Update()
    {
        base.Update();

        if (_isPrimaryActive)
        {
            PrimaryAttack();
        }
    }
    /// <summary>
    /// Mage player's primary attack
    /// </summary>
    protected override void PrimaryAttack()
    {
        // Ensure player has enough mana to perform this attack
        if ((Stats as PlayerStatsController).mana.CurrentValue < 1f)
        {
            // Stop attack if not enough mana
            ReleaseChargedAttack();
            return;
        }

        (Stats as PlayerStatsController).mana.Subtract(primaryAttackManaDepletion * Time.deltaTime);

        Vector3 vfxPos = transform.position + transform.forward * 1.6f + new Vector3(0, 2f);
        StartCoroutine(VfxHelper.CreateVFX(primaryVFX, vfxPos, transform.rotation));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        float baseDamage = Stats.damage.GetValue();
        float damageValue = Random.Range(primaryAttackMinDamage + baseDamage, primaryAttackMaxDamage + baseDamage);

        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackSweepAngle)))
        {    
            enemy.GetComponent<EntityStatsController>().TakeDamage(Stats, damageValue, Time.deltaTime);
        }
    }
    /// <summary>
    /// Mage player's secondary attack
    /// </summary>
    protected override void SecondaryAttack()
    {
        // Ensure player has enough mana to perform this attack
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < secondaryAttackManaDepletion)
            return;

        StartCoroutine(VfxHelper.CreateVFX(secondaryVFX, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity, PlayerManager.colours.GetColour(Stats.characterColour)));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        float baseDamage = Stats.damage.GetValue();
        float damage = secondaryAttackMaxDamage + baseDamage;

        // Attack any enemies within the AOE range
        foreach (var enemy in enemies)
        {
            StartCoroutine(PerformExplosiveDamage(enemy.GetComponent<EntityStatsController>(),
                                                          damage, secondaryAttackStunTime, secondaryAttackExplosionForce,
                                                          transform.position, secondaryAttackRadius, secondaryAttackDamageDelay));
        }

        // Trigger secondary attack animation
        Anim.SetTrigger("SecondaryAttack");

        // Play the attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, secondaryAttackWeaponSFX));

        // Reset attack timeout and deplete mana
        AttackCooldown = secondaryAttackTimeout;
        (Stats as PlayerStatsController).mana.Subtract(secondaryAttackManaDepletion);

        // Apply movement speed modifier
        StartCoroutine(Motor.ApplyTimedMovementModifier(secondaryAttackMovementModifier, secondaryAttackTimeout));
    }
    /// <summary>
    /// Mage player's ultimate attack
    /// </summary>
    protected override void UltimateAbility()
    {
        Anim.SetTrigger("UltimateAbility");
    }
    /// <summary>
    /// Toggle the primary attack based on the input value
    /// </summary>
    /// <param name="value">Value of the input controller primary attack button state</param>
    protected override void OnPrimaryAttack(InputValue value)
    {
        if (AttackCooldown > 0) return;
        
        if (value.isPressed)
        {
            Interaction.StopInteract();
            (Stats as PlayerStatsController).mana.Subtract(primaryAttackManaDepletion);
            _isPrimaryActive = true;
            Anim.SetBool("PrimaryAttack", true);
            StartCoroutine(AudioHelper.PlayAudio(WeaponAudio, primaryAttackWeaponSFX));
            (Stats as PlayerStatsController).mana.PauseRegen();
            Motor.ApplyMovementModifier(primaryAttackMovementModifier);
        }
        else
        {
            _isPrimaryActive = false;
            Anim.SetBool("PrimaryAttack", false);
            AudioHelper.StopAudio(WeaponAudio);
            (Stats as PlayerStatsController).mana.StartRegen();
            Motor.ResetMovementModifier();
        }
    }
    /// <summary>
    /// Perform the secondary attack based on the input value
    /// </summary>
    /// <param name="value">Value of the input controller secondary attack button state</param>
    protected override void OnSecondaryAttack(InputValue value)
    {
        // don't attack if the primary attack is active
        if (!_isPrimaryActive && value.isPressed)
        {
            Interaction.StopInteract();
            SecondaryAttack();
        }
    }

    protected override void ReleaseChargedAttack()
    {
        _isPrimaryActive = false;
        Anim.SetBool("PrimaryAttack", false);
        AudioHelper.StopAudio(WeaponAudio);
        (Stats as PlayerStatsController).mana.StartRegen();
        Motor.ResetMovementModifier();
    }
}