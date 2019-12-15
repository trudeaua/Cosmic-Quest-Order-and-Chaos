using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Class for player-lever interaction
public class Lever : Interactable
{
    protected Animator Anim;
    protected Room Room;
    public bool IsPulled;
    private AudioSource audioClip;

    private void Start()
    { 
        // Find the door of the room that lever is in
        Room = transform.parent.parent.Find("Door").GetComponent<Room>();
        
        Anim = gameObject.GetComponent<Animator>();
        audioClip = gameObject.GetComponent<AudioSource>(); 
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
            
            audioClip.Play(0);
            IsPulled = true;

            // Add lever colour to the code input
            Room.input.Add(this.colour);
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
