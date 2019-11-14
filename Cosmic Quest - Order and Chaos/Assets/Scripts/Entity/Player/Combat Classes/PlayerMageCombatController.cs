using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMageCombatController : PlayerCombatController
{
    [Tooltip("The distance the primary projectile attack can travel")]
    public float primaryAttackRange = 20f;
    [Tooltip("The velocity that the primary projectile attack will travel at")]
    public float primaryAttackVelocity = 10f;
    [Tooltip("The projectile prefab for the primary attack")]
    public GameObject primaryProjectilePrefab;
    [Tooltip("The distance the secondary attack will reach")]
    public float secondaryAttackRange = 3f;
    [Tooltip("The secondary attack projected angle of AOE in degrees")]
    public float secondaryAttackAngle = 45f;
    
    private bool _isSecondaryActive = false;

    protected override void Update()
    {
        base.Update();

        if (_isSecondaryActive)
        {
            SecondaryAttack();
        }
    }

    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        
        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(primaryProjectilePrefab, transform.forward, primaryAttackVelocity, primaryAttackRange, 0.5f));
    }
    
    protected override void SecondaryAttack()
    {
        Debug.Log("Firing");
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        // Ensure secondary is only activated on button down
        _isSecondaryActive = value.isPressed;
    }
}
