using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyBrainController))]
public class EnemyCombatController : EntityCombatController
{
    [Header("General Settings")]
    public bool hasSpecialAttack;
    [Tooltip("How often the enemy will perform its special attack if it has one")]
    public float specialAttackPeriod;

    public bool IsCoolingDown => AttackCooldown > 0f;
    protected float SpecialAttackTimer;
    public bool CanUseSpecialAttack => hasSpecialAttack && SpecialAttackTimer <= 0f;

    protected EnemyBrainController Brain;
    protected IEnumerable<GameObject> Players => PlayerManager.Instance.Players;

    protected override void Awake()
    {
        base.Awake();

        Brain = GetComponent<EnemyBrainController>();
    }
    
    private void Start()
    {
        // Initialize special attack timer
        if (hasSpecialAttack)
            SpecialAttackTimer = specialAttackPeriod;
    }
    
    protected override void Update()
    {
        base.Update();

        if (hasSpecialAttack && SpecialAttackTimer > 0)
            SpecialAttackTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Placeholder for enemy primary attack
    /// </summary>
    public virtual void PrimaryAttack()
    {
        Debug.Log(gameObject.name + "'s primary attack triggered");
    }
    
    /// <summary>
    /// Placeholder for enemy secondary attack
    /// </summary>
    public virtual void SecondaryAttack()
    {
        Debug.Log(gameObject.name + "'s secondary attack triggered");
    }
    
    /// <summary>
    /// Placeholder for enemy tertiary attack
    /// </summary>
    public virtual void TertiaryAttack()
    {
        Debug.Log(gameObject.name + "'s tertiary attack triggered");
    }

    /// <summary>
    /// Placeholder for enemy special attack starter
    /// </summary>
    public virtual void SpecialAttack()
    {
        Debug.Log(gameObject.name + "'s special attack triggered");
    }

    /// <summary>
    /// Selects an attack to perform based on enemy's attack strategy
    /// </summary>
    public virtual void ChooseAttack()
    {
        Debug.Log("Default ChooseAttack() implementation triggered");
    }

    /// <summary>
    /// Determines if the enemy can deal damage to a player
    /// </summary>
    /// <param name="target">The GameObject of the target player</param>
    /// <param name="radius">The range of the attack</param>
    /// <param name="sweepAngle">The angular distance in degrees of the attacks FOV.
    /// If set to 360 or left unset then the enemy can attack in any direction.</param>
    /// <returns>Whether the enemy can damage the player</returns>
    public bool CanDamageTarget(GameObject target, float radius, float sweepAngle = 360f)
    {
        // TODO need to rethink hitboxes or standardize projecting from y = 1
        Vector3 pos = transform.position;
        Vector3 targetPos = target.transform.position;
        pos.y = 1f;
        Vector3 rayDirection = targetPos - pos;
        rayDirection.y = 0;

        // Player should be within the radius
        if (Vector3.Distance(pos, targetPos) > radius)
            return false;

        if (Mathf.Approximately(sweepAngle, 360f) || Vector3.Angle(rayDirection, transform.forward) <= sweepAngle * 0.5f)
        {
            // Check if enemy is within player's sight
            if (Physics.Raycast(pos, rayDirection, out RaycastHit hit, radius))
            {
                return hit.transform.gameObject.Equals(target);
            }
        }

        return false;
    }
}