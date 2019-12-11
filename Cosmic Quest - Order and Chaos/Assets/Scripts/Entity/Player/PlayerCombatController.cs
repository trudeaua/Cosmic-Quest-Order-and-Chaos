using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStatsController))]
public class PlayerCombatController : EntityCombatController
{
    [Tooltip("The minimal cooldown between consecutive attacks. This value should be larger than the time it takes for the entire attack animation")]
    public float primaryAttackTimeout;
    [Tooltip("The minimal cooldown between consecutive attacks. This value should be larger than the time it takes for the entire attack animation")]
    public float secondaryAttackTimeout;
    
    public GameObject spawnVFX;

    protected virtual void Start()
    {
        // Create a VFX where the player will spawn - just slightly above the stage (0.1f) - and change the VFX colour to match the player colour
        StartCoroutine(CreateVFX(spawnVFX, new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z), 
            Quaternion.identity, PlayerManager.colours.GetColour(Stats.characterColour), 0.5f));
        // "Spawn" the player (they float up through the stage)
        StartCoroutine(Spawn(gameObject, 0.08f, 0.9f));
    }

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
