using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStatsController))]
public class PlayerCombatController : EntityCombatController
{
    [Tooltip("The minimal cooldown between consecutive attacks. This value should be larger than the time it takes for the entire attack animation")]
    public float primaryAttackTimeout;
    [Tooltip("The minimal cooldown between consecutive attacks. This value should be larger than the time it takes for the entire attack animation")]
    public float secondaryAttackTimeout;

    protected PlayerMotorController Motor;
    protected PlayerInteractionController Interaction;

    protected virtual void Start()
    {
        Motor = GetComponent<PlayerMotorController>();
        Interaction = GetComponent<PlayerInteractionController>();
    }
    /// <summary>
    /// Player's primary attack placeholder
    /// </summary>
    protected virtual void PrimaryAttack()
    {
        // Implement me
        Debug.Log("Player's base primary attack triggered");
    }
    /// <summary>
    /// Player's secondary attack placeholder
    /// </summary>
    protected virtual void SecondaryAttack()
    {
        // Implement me
        Debug.Log("Player's base secondary attack triggered");
    }
    /// <summary>
    /// Player's ultimate attack placeholder
    /// </summary>
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
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, radius, Hits, EntityStatsController.EntityLayer);

        for (int i = 0; i < numHits; i++)
        {
            if (Hits[i].transform.CompareTag("Enemy"))
            {
                enemies.Add(Hits[i].transform);
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
    /// <returns>Whether the player can damage the enemy</returns>
    protected bool CanDamageTarget(Transform target, float radius, float sweepAngle = 360f)
    {
        // TODO need to rethink hitboxes or standardize projecting from y = 1
        Vector3 pos = transform.position;
        pos.y = 1f;
        Vector3 rayDirection = target.position - pos;
        rayDirection.y = 0;

        if (Mathf.Approximately(sweepAngle, 360f) || Vector3.Angle(rayDirection, transform.forward) <= sweepAngle * 0.5f)
        {
            // Check if enemy is within player's sight
            if (Physics.Raycast(pos, rayDirection, out RaycastHit hit, radius))
            {
                return hit.transform == target;
            }
        }

        return false;
    }
    /// <summary>
    /// Play an attack animation for a certain amount of time
    /// </summary>
    /// <param name="animName">Attack animation state name</param>
    /// <param name="time">Number of seconds to wait before stopping the animation</param>
    protected IEnumerator TriggerTimeAttackAnimation(string animName, float time)
    {
        Anim.SetBool(animName, true);
        yield return new WaitForSeconds(time);
        Anim.SetBool(animName, false);
    }
    /// <summary>
    /// Toggle the primary attack based on the input value
    /// </summary>
    /// <param name="value">Value of the input controller primary attack button state</param>
    protected virtual void OnPrimaryAttack(InputValue value)
    {
        // Only trigger attack on button down by default
        if (value.isPressed)
        {
            Interaction.StopInteract();
            PrimaryAttack();
        }
    }
    /// <summary>
    /// Toggle the secondary attack based on the input value
    /// </summary>
    /// <param name="value">Value of the input controller secondary attack button state</param>
    protected virtual void OnSecondaryAttack(InputValue value)
    {
        // Only trigger attack on button down by default
        if (value.isPressed)
        {
            Interaction.StopInteract();
            SecondaryAttack();
        }
    }
    /// <summary>
    /// Toggle the ultimate attack based on the input value
    /// </summary>
    /// <param name="value">Value of the input controller ultimate attack button state</param>
    protected virtual void OnUltimateAbility(InputValue value)
    {
        // Only trigger ability on button down by default
        if (value.isPressed)
        {
            UltimateAbility();
        }
    }

    protected virtual void ReleaseChargedAttack()
    {
        Debug.Log("Cancelled charge attack");
    }
}