using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for player interactable objects
/// </summary>
public class Interactable : MonoBehaviour
{
    [Tooltip("Max distance a transform can be to interact with this")]
    public float radius = 3f;

    [Tooltip("Whether the player must hold down the interact button to interact with this object")]
    public bool isHeld = false;

    [Tooltip("Whether the interaction is meant to be a triggered-style interaction")]
    public bool isTrigger = false;
    
    [Tooltip("Required character colour to interact with")]
    public CharacterColour colour = CharacterColour.All;

    /// <summary>
    /// Handles the start of an interaction event with a player
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public virtual void StartInteract(Transform target)
    {
        // This function is intended to be overriden
        if (CanInteract(target))
        {
            Debug.Log("Started interaction with " + target.name);
        }
    }
    
    /// <summary>
    /// Handles the end of the interaction event with a player. This function is not necessary for one-time interactions.
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public virtual void StopInteract(Transform target)
    {
        // This function is intended to be overriden
        Debug.Log("Stopped interaction with " + target.name);
    }

    /// <summary>
    /// Determines if a given player is able to interact with this object
    /// </summary>
    /// <param name="target">The Transform attempting to interact with this object</param>
    /// <returns>Whether the Transform can interact with this object</returns>
    public virtual bool CanInteract(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= radius &&
               (colour == CharacterColour.All || target.GetComponent<EntityStatsController>().characterColour == colour);
    }
    
    // Displays the interaction radius in the editor
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
