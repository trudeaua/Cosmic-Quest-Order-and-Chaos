using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for player-lever interaction
public class Lever : Interactable
{
    protected Animator Anim;

    private void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
    }

    private void Reset()
    {
        // Set default value for interactable fields
        isTrigger = true;
    }
    
    public override void StartInteract(Transform target)
    {
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
            Anim.enabled = true;
            Anim.Play("LeverAnimation");
            Anim.SetBool("LeverPulled", true);
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
