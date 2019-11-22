using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [Tooltip("Max distance of objects that the player can interact with")]
    public float interactionRadius = 4f;
    
    protected Interactable CurrentObject = null;

    /// <summary>
    /// Used to check if the player is currently interacting with something. This is mostly meant to be used for
    /// player state checking within the other Controller classes.
    /// </summary>
    /// <returns>Whether the player is interacting.</returns>
    public bool IsInteracting()
    {
        return (CurrentObject);
    }
    
    private void OnInteract(InputValue value)
    {
        // Only trigger on button pressed down
        if (value.isPressed)
        {
            // If currently interacting with a "non-held" object, stop interacting
            if (CurrentObject)
            {
                CurrentObject.StopInteract(transform);
                CurrentObject = null;
                return;
            }
            
            // Attempt to interact with the first interactable in the player's view
            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactionRadius))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable is null)
                    return;
                
                // Attempt interaction
                interactable.StartInteract(transform);
                
                if (!interactable.isTrigger)
                    CurrentObject = interactable;
            }
        }
        else if (CurrentObject && CurrentObject.isHeld)
        {
            // Stop interacting with a "held" object
            CurrentObject.StopInteract(transform);
            CurrentObject = null;
        }
    }
}
