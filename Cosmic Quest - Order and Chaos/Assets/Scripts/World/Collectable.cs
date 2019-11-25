using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for items which are collected on interact
/// </summary>
public class Collectable : Interactable //inherits from interactable
{
    /// <summary>
    /// Handles the start of an interaction event with a player
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public override void StartInteract(Transform target)
    {
        // This function is intended to be overriden
        if (CanInteract(target))
        {
            Debug.Log("Picked up by " + target.name);
            //TODO: Add functionality later
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Handles the end of the interaction event with a player. This function is not necessary for one-time interactions.
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public override void StopInteract(Transform target)
    {
        // This function is intended to be overriden
        Debug.Log("WARNING: stopinteract on Collectable shouldn't be reachable");
    }

    
}
