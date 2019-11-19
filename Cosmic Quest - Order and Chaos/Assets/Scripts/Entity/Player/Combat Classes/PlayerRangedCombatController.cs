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
                _isPrimaryCharging = false;
            }
        }
    }

    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = primaryAttackCooldown;

        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(primaryProjectilePrefab, transform.forward, _primaryAttackLaunchForce, primaryAttackRange, 0.3f));
        
        // Bow release animation
        Anim.SetTrigger("PrimaryAttack");
    }
    
    protected override void SecondaryAttack()
    {
        // TODO implement ranger's secondary attack
        Anim.SetTrigger("SecondaryAttack");
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
            _isPrimaryCharging = true;
            _primaryChargeTime = 0f;
        }
        else
        {
            _isPrimaryCharging = false;
            
            // Convert charge time to launch force
            _chargePercent = Mathf.InverseLerp(0f, primaryAttackChargeTime,_primaryChargeTime);
            _primaryAttackLaunchForce = Mathf.Lerp(primaryAttackMinLaunchForce, primaryAttackMaxLaunchForce, _chargePercent);
            PrimaryAttack();
        }
        Anim.SetBool("Charge", _isPrimaryCharging);
    }
}
