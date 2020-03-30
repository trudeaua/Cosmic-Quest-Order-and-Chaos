using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [Tooltip("Max distance of objects that the player can interact with")]
    public float interactionRadius = 4f;

    private PlayerCombatController _combat;
    private PlayerStatsController _stats;
    private Interactable _currentObject = null;
    private Animator _anim;

    private void Start()
    {
        _combat = GetComponent<PlayerCombatController>();
        _anim = gameObject.GetComponentInChildren<Animator>();
        _stats = GetComponent<PlayerStatsController>();
        _stats.onDeath.AddListener(StopInteract);
    }

    /// <summary>
    /// Used to check if the player is currently interacting with something. This is mostly meant to be used for
    /// player state checking within the other Controller classes.
    /// </summary>
    /// <returns>Whether the player is interacting.</returns>
    public bool IsInteracting()
    {
        return (_currentObject);
    }

    public void StopInteract()
    {
        if (!_currentObject)
        {
            return;
        }
        // Decide which animation to do
        if (_currentObject is Draggable)
        {
            _anim.SetBool("PickedUp", false);
        }

        _currentObject.StopInteract(transform);
        _currentObject = null;
    }


    /// <summary>
    /// Toggle the interaction sequence
    /// </summary>
    /// <param name="value">Value of the input controller interact button state</param>
    private void OnInteract(InputValue value)
    {
        // Only trigger on button pressed down
        if (value.isPressed)
        {
            // If currently interacting with a "non-held" object, stop interacting
            if (_currentObject)
            {
                StopInteract();
                return;
            }
            
            // Ensure the player isn't currently attacking before attempting interaction
            if (_combat.AttackCooldown > 0)
                return;
            
            // Attempt to interact with the first interactable in the player's view
            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactionRadius))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable is null)
                    return;

                if (!interactable.CanInteract(transform))
                {
                    return;
                }

                // Decide which animation to do
                if (interactable is Draggable)
                {
                    _anim.SetBool("PickedUp", true);
                }
                else if (interactable is Lever)
                {
                    _anim.SetTrigger("InteractStanding");
                }
                else if (interactable is Collectable)
                {
                    _anim.SetTrigger("InteractGround");
                }

                // Attempt interaction
                interactable.StartInteract(transform);
                
                if (!interactable.isTrigger)
                    _currentObject = interactable;
            }
        }
        else if (_currentObject && _currentObject.isHeld)
        {
            // Stop interacting with a "held" object
            StopInteract();
        }
    }
}
