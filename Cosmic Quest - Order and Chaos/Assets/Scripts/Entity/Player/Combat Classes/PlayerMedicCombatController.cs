using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMedicCombatController : PlayerCombatController
{
    [Header("Primary Attack - Wide Swipe")]
    [Tooltip("The minimum base damage that this attack can deal")]
    public float primaryAttackMinDamage = 0f;
    [Tooltip("The maximum base damage that this attack can deal")]
    public float primaryAttackMaxDamage = 5f;
    [Tooltip("The maximum range the player's melee attack can reach")]
    public float primaryAttackRadius = 4f;
    [Tooltip("The angular sweep in front of the player where enemies are affected by the attack")]
    [Range(0f, 360f)]
    public float primaryAttackSweepAngle = 100f;
    [Tooltip("The delay before damage is applied to enemies. This is to sync up with the animation")]
    public float primaryAttackDamageDelay = 0.6f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float primaryAttackManaDepletion = 25f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float primaryAttackMovementModifier = 0.5f;
    [Tooltip("Weapon audio effect for primary attack")]
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackWeaponSFX;
    
    [Header("Secondary Attack - Healing Orb")]
    [Tooltip("The minimum base damage that this attack can deal")]
    public float secondaryAttackMinDamage = 0f;
    [Tooltip("The maximum base damage that this attack can deal")]
    public float secondaryAttackMaxDamage = 5f;
    [Tooltip("The minimum base healing that this attack can deal")]
    public float secondaryAttackMinHealing = 10f;
    [Tooltip("The maximum base healing that this attack can deal")]
    public float secondaryAttackMaxHealing = 15f;
    [Tooltip("The force to launch the healing projectile at")]
    public float secondaryAttackLaunchForce = 500f;
    [Tooltip("The range which the healing projectile can travel")]
    public float secondaryAttackRange = 20f;
    [Tooltip("The delay before the projectile is launched")]
    public float secondaryAttackLaunchDelay = 0.5f;
    [Tooltip("The amount of the player's mana depleted (and necessary) per attack")]
    public float secondaryAttackManaDepletion = 30f;
    [Tooltip("The percent modifier of movement speed during this attack")]
    [Range(0f, 1f)]
    public float secondaryAttackMovementModifier = 0.5f;
    [Tooltip("The prefab for the healing projectile")]
    public GameObject projectilePrefab;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackWeaponSFX;
    /// <summary>
    /// Healer's primary attack
    /// </summary>
    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < primaryAttackManaDepletion)
            return;

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        float baseDamage = Stats.damage.GetValue();

        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackSweepAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(primaryAttackMinDamage + baseDamage, primaryAttackMaxDamage + baseDamage);
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), damageValue, primaryAttackDamageDelay));
        }

        // Trigger primary attack animation
        StartCoroutine(TriggerTimeAttackAnimation("PrimaryAttack", primaryAttackTimeout));

        // Play the attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, primaryAttackWeaponSFX));

        // Reset attack timeout and deplete mana
        AttackCooldown = primaryAttackTimeout;
        (Stats as PlayerStatsController).mana.Subtract(primaryAttackManaDepletion);

        // Apply movement speed modifier
        StartCoroutine(Motor.ApplyTimedMovementModifier(primaryAttackMovementModifier, primaryAttackTimeout));
    }
    /// <summary>
    /// Healer's secondary attack
    /// </summary>
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0 || (Stats as PlayerStatsController).mana.CurrentValue < secondaryAttackManaDepletion)
            return;

        float baseDamage = Stats.damage.GetValue();

        float damageValue = Random.Range(secondaryAttackMinDamage + baseDamage, secondaryAttackMaxDamage + baseDamage);
        float healingValue = Random.Range(secondaryAttackMinHealing, secondaryAttackMaxHealing);

        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(projectilePrefab, transform.forward, secondaryAttackLaunchForce, secondaryAttackRange, damageValue, healingValue, secondaryAttackLaunchDelay));

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

    private IEnumerator LaunchProjectile(GameObject projectilePrefab, Vector3 direction, float launchForce, float range, float damage, float heal, float launchDelay = 0f)
    {
        if (launchDelay > 0f)
            yield return new WaitForSeconds(launchDelay);

        // Launch projectile from projectile pool
        GameObject projectile = ObjectPooler.Instance.GetPooledObject(projectilePrefab);
        projectile.GetComponent<HealerProjectile>().Launch(Stats, direction, launchForce, range, damage, heal);
    }

    /// <summary>
    /// Healer's ultimate attack
    /// </summary>
    protected override void UltimateAbility()
    {
        Anim.SetTrigger("UltimateAbility");
    }
}