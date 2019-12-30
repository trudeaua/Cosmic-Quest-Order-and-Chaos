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
    private AudioSource _audioClip;

    private void Start()
    { 
        // Find Room that lever is in
        Room = transform.parent.parent.gameObject.GetComponent<Room>();
        
        Anim = GetComponent<Animator>();
        _audioClip = GetComponent<AudioSource>(); 
    }

    /// <summary>
    /// Reset the lever trigger
    /// </summary>
    private void Reset()
    {
        // Set default value for interactable fields
        isTrigger = true;
    }
    
    /// <summary>
    /// Start interaction with the lever
    /// </summary>
    /// <param name="target">Target that is trying to interact with the lever</param>
    public override void StartInteract(Transform target)
    {
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
            
            Anim.enabled = true;
            Anim.Play("LeverAnimation");
            Anim.SetBool("LeverPulled", true);
            
            _audioClip.PlayDelayed(0);
            IsPulled = true;

            // Add lever colour to the code input
            Room.Input.Add(this.colour);
        }
    }

    /// <summary>
    /// Pause the animation of the lever
    /// </summary>
    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
