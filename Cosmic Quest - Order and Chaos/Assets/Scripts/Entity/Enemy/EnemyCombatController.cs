using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyBrainController))]
public class EnemyCombatController : EntityCombatController
{
    public float primaryAttackCooldown = 1f;
    public float primaryAttackDelay = 0.6f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;

    public float attackRadius = 3f;
    public float attackAngle = 45f;

    protected List<GameObject> Players;

    private void Start()
    {
        Players = PlayerManager.Players;
    }

    public virtual void PrimaryAttack()
    {
        Debug.Log(gameObject.name + "'s primary attack triggered");
    }

    public virtual void SecondaryAttack()
    {
        Debug.Log(gameObject.name + "'s secondary attack triggered");
    }

    public virtual void TertiaryAttack()
    {
        Debug.Log(gameObject.name + "'s tertiary attack triggered");
    }

    /// <summary>
    /// Determines if the enemy can deal damage to a player
    /// </summary>
    /// <param name="target">The position of the target player</param>
    /// <param name="radius">The range of the attack</param>
    /// <param name="sweepAngle">The angular distance in degrees of the attacks FOV.
    /// If set to 360 or left unset then the enemy can attack in any direction.</param>
    /// <returns>Whether the enemy can damage the player</returns>
    protected bool CanDamageTarget(Vector3 target, float radius, float sweepAngle = 360f)
    {
        // TODO need to rethink hitboxes or standardize projecting from y = 1
        Vector3 pos = transform.position;
        pos.y = 1f;
        Vector3 rayDirection = target - pos;
        rayDirection.y = 0;

        if (Mathf.Approximately(sweepAngle, 360f) || Vector3.Angle(rayDirection, transform.forward) <= sweepAngle * 0.5f)
        {
            // Check if enemy is within player's sight
            if (Physics.Raycast(pos, rayDirection, out RaycastHit hit, radius))
            {
                return hit.transform.CompareTag("Player");
            }
        }

        return false;
    }
}