﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for player interactable objects
/// </summary>
public class Interactable : MonoBehaviour
{
    [Tooltip("Max distance a transform can be to interact with this")]
    public float radius = 3f;

    [Tooltip("Required character colour to interact with")]
    public CharacterColour colour = CharacterColour.None;

    /// <summary>
    /// Handles the interaction event when interacted with a player
    /// </summary>
    /// <param name="target">The Transform which interacted with this object</param>
    public virtual void Interact(Transform target)
    {
        // This function is intended to be overriden
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
        }
    }

    /// <summary>
    /// Determines if a given player is able to interact with this object
    /// </summary>
    /// <param name="target">The Transform attempting to interact with this object</param>
    /// <returns>Whether the Transform can interact with this object</returns>
    public virtual bool CanInteract(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= radius &&
               (colour == CharacterColour.None || target.GetComponent<EntityStatsController>().characterColour == colour);
    }
    
    // Displays the interaction radius in the editor
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}