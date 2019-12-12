using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedCombatController : PlayerCombatController
{
    [Header("Primary Attack")]
    [Tooltip("The distance the primary attack arrow can travel")]
    public float primaryAttackRange = 20f;
    [Tooltip("Time in seconds to charge primary attack to full power")]
    public float primaryAttackChargeTime = 1.5f;
    [Tooltip("The minimum force to launch the primary attack projectile at")]
    public float primaryAttackMinLaunchForce = 150f;
    [Tooltip("The maximum force to launch the primary attack projectile at")]
    public float primaryAttackMaxLaunchForce = 800f;
    [Tooltip("The arrow prefab for the primary attack")]
    public GameObject primaryProjectilePrefab;
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float primaryAttackDelay = 0.3f;
    [Tooltip("How long until the player can attack after the primary attack")]
    public float primaryAttackCooldown;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip primaryAttackChargeWeaponSFX;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip primaryAttackReleaseWeaponSFX;

    [Header("Secondary Attack")]
    [Tooltip("How long until the player can attack after the secondary attack")]
    public float secondaryAttackCooldown;
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float secondaryAttackDelay = 0.5f;
    [Tooltip("The trap prefab for the secondary attack")]
    public GameObject secondaryTrapPrefab;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip secondaryAttackWeaponSFX;
    [Tooltip("Weapon audio effect for secondary attack explosion")]
    [SerializeField] protected EntityAudioClip secondaryAttackWeaponExplosionSFX;
    
    private bool _isPrimaryCharging;
    private float _primaryChargeTime;
    private float _chargePercent;
    private float _primaryAttackLaunchForce;

    protected override void Update()
    {
        base.Update();
        
        // Handle attack charge ups
        if (_isPrimaryCharging)
        {
            _primaryChargeTime += Time.deltaTime;
            if (_primaryChargeTime > primaryAttackChargeTime)
            {
                // Set to max charge time and stop counting charge time
                _primaryChargeTime = primaryAttackChargeTime;
                //_isPrimaryCharging = false;
            }
        }
    }

    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;
        AttackCooldown = primaryAttackCooldown;

        float damage = Mathf.Ceil(Stats.damage.GetValue() * _chargePercent);
        
        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(primaryProjectilePrefab, transform.forward, _primaryAttackLaunchForce, primaryAttackRange, damage, primaryAttackDelay));
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        
        // Place explosive trap
        StartCoroutine(PlaceTrap(secondaryTrapPrefab, secondaryAttackDelay));
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
        Anim.SetTrigger("UltimateAbility");
    }

    private IEnumerator LaunchProjectile(GameObject projectilePrefab, Vector3 direction, float launchForce, float range, float damage, float launchDelay = 0f)
    {
        ResetTakeDamageAnim();
        if (launchDelay > 0f)
            yield return new WaitForSeconds(launchDelay);
        
        // Launch projectile from projectile pool
        GameObject projectile = ObjectPooler.Instance.GetPooledObject(projectilePrefab);
        projectile.GetComponent<DamageProjectile>().Launch(Stats, direction, launchForce, range, damage);
    }

    private IEnumerator PlaceTrap(GameObject trapPrefab, float spawnDelay = 0f)
    {
        ResetTakeDamageAnim();
        if (spawnDelay > 0f)
            yield return new WaitForSeconds(spawnDelay);
        
        // Place trap from object pool in front of the player
        GameObject trap = ObjectPooler.Instance.GetPooledObject(trapPrefab);
        ExplosiveTrap explosiveTrap = trap.GetComponent<ExplosiveTrap>();
        explosiveTrap.SetAudio(secondaryAttackWeaponExplosionSFX);
        explosiveTrap.PlaceTrap(Stats, transform.position + transform.forward);
    }

    protected override void OnPrimaryAttack(InputValue value)
    {
        bool isPressed = value.isPressed;
        if (isPressed && AttackCooldown <= 0 && !_isPrimaryCharging)
        {
            _isPrimaryCharging = true;
            _primaryChargeTime = 0f;
            StartCoroutine(Stats.PlayAudioOverlap(primaryAttackChargeWeaponSFX));
            Anim.SetBool("PrimaryAttack", true);
        }
        else if (!isPressed && AttackCooldown <= 0 && _isPrimaryCharging)
        {
            _isPrimaryCharging = false;
            // Convert charge time to launch force
            _chargePercent = Mathf.InverseLerp(0f, primaryAttackChargeTime,_primaryChargeTime);
            _primaryAttackLaunchForce = Mathf.Lerp(primaryAttackMinLaunchForce, primaryAttackMaxLaunchForce, _chargePercent);
            Stats.StopAudio(primaryAttackChargeWeaponSFX);
            StartCoroutine(Stats.PlayAudioOverlap(primaryAttackReleaseWeaponSFX));
            PrimaryAttack();
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
                StartCoroutine(Stats.PlayAudioOverlap(secondaryAttackWeaponSFX));
                Anim.SetTrigger("SecondaryAttack");
                SecondaryAttack();
            }
        }
    }
}
