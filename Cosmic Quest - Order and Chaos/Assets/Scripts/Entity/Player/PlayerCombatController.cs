using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : EntityCombatController
{
    [SerializeField] protected float primaryAttackCooldown = 0.5f;
    [SerializeField] protected float secondaryAttackCooldown = 1f;
    
    protected virtual void PrimaryAttack()
    {
        // Implement me
        Debug.Log("Player's base primary attack triggered");
    }

    protected virtual void SecondaryAttack()
    {
        // Implement me
        Debug.Log("Player's base secondary attack triggered");
    }

    protected virtual void UltimateAbility()
    {
        // Implement me
        Debug.Log("Player's base ultimate ability triggered");
    }
    
    /// <summary>
    /// Returns all the enemies surrounding the player within a given radius
    /// </summary>
    /// <param name="radius">The radius around the player to check</param>
    /// <returns>A list of the surrounding enemy transforms</returns>
    protected List<Transform> GetSurroundingEnemies(float radius)
    {
        List<Transform> enemies = new List<Transform>();
        Collider[] hits = new Collider[32]; // TODO is 32 hits an okay amount to initialize to?
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, radius, hits, EntityLayer);

        for (int i = 0; i < numHits; i++)
        {
            if (hits[i].transform.CompareTag("Enemy"))
            {
                enemies.Add(hits[i].transform);
            }
        }

        return enemies;
    }
    
    /// <summary>
    /// Determines if the player can deal damage to an enemy
    /// </summary>
    /// <param name="target">The position of the target enemy</param>
    /// <param name="radius">The range of the attack</param>
    /// <param name="sweepAngle">The angular distance in degrees of the attacks FOV.
    /// If set to 360 or left unset then the player can attack in any direction.</param>
    /// <returns></returns>
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
                return hit.transform.CompareTag("Enemy");
            }
        }

        return false;
    }

    protected virtual void OnPrimaryAttack(InputValue value)
    {
        // Only trigger attack on button down by default
        if (value.isPressed)
        {
            PrimaryAttack();
        }
    }
    
    protected virtual void OnSecondaryAttack(InputValue value)
    {
        // Only trigger attack on button down by default
        if (value.isPressed)
        {
            SecondaryAttack();
        }
    }

    protected virtual void OnUltimateAbility(InputValue value)
    {
        // Only trigger ability on button down by default
        if (value.isPressed)
        {
            UltimateAbility();
        }
    }
}
