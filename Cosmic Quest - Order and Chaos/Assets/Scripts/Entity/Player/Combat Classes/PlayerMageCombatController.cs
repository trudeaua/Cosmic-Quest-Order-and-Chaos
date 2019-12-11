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
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public int primaryAttackDelay = 3;
    [Tooltip("How long until the player can attack after the primary attack")]
    public float primaryAttackCooldown;
    [Tooltip("Visual effect for primary attack")]
    public GameObject primaryVFX;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip primaryAttackWeaponSFX;

    [Header("Secondary Attack")]
    [Tooltip("The radius of the AOE effect")]
    public float secondaryAttackRadius = 8f;
    [Tooltip("The explosive force of the AOE effect")]
    public float secondaryAttackForce = 500f;
    [Tooltip("How long until the player can attack after the secondary attack")]
    public float secondaryAttackCooldown;
    [Tooltip("Time between when attack starts vs when damage is dealt")]
    public float secondaryAttackDelay = 0.6f;
    [Tooltip("Visual effect for secondary attack")]
    public GameObject secondaryVFX;
    [Tooltip("Weapon audio effect for secondary attack")]
    [SerializeField] protected EntityAudioClip secondaryAttackWeaponSFX;


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
        ResetTakeDamageAnim();
        Vector3 vfxPos = gameObject.transform.position + gameObject.transform.forward * 1.5f + new Vector3(0, 2f);
        StartCoroutine(CreateVFX(primaryVFX, vfxPos, gameObject.transform.rotation));
        if (AttackCooldown > 0) return;
        AttackCooldown = primaryAttackDelay * Time.deltaTime;

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRange);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRange, primaryAttackAngle)))
        {
            float baseDamage = Stats.damage.GetValue() / 5f;
            float damage = Random.Range(baseDamage * 0.9f, baseDamage * 1.1f);
            enemy.GetComponent<EntityStatsController>().TakeDamage(Stats, damage);
        }
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        StartCoroutine(CreateVFX(secondaryVFX, gameObject.transform.position, Quaternion.identity, PlayerManager.colours.GetColour(Stats.characterColour)));

        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(secondaryAttackRadius);
        
        // Attack any enemies within the AOE range
        foreach (var enemy in enemies)
        {
            StartCoroutine(PerformExplosiveDamage(enemy.GetComponent<EntityStatsController>(), 
                Stats.damage.GetValue(), 2f, secondaryAttackForce, transform.position, secondaryAttackRadius, secondaryAttackDelay));
        }
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
        Anim.SetTrigger("UltimateAbility");
    }

    protected override void OnPrimaryAttack(InputValue value)
    {
        _isPrimaryActive = value.isPressed && AttackCooldown <= 0;
        if (_isPrimaryActive)
        {
            StartCoroutine(Stats.PlayAudio(primaryAttackWeaponSFX));
        }
        else
        {
            Stats.StopAudio();
        }
        Anim.SetBool("PrimaryAttack", _isPrimaryActive);
    }

    protected override void OnSecondaryAttack(InputValue value)
    {
        bool isPressed = value.isPressed;
        if (AttackCooldown <= 0 && !_isPrimaryActive)
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
