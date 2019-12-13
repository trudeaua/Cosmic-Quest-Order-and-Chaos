using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Class for player-lever interaction
public class Lever : Interactable
{
    protected Animator Anim;
    public GameObject Door;

    private void Start()
    { 
        // Find the door of the room that lever is in
        Door = transform.parent.parent.Find("Door1").gameObject;
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
            Anim.enabled = true;
            Anim.Play("LeverAnimation");
            Anim.SetBool("LeverPulled", true);

            // Add lever colour to the code input
            Door.GetComponent<Room>().input.Add(this.colour);
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
