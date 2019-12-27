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
            
            _audioClip.Play(0);
            IsPulled = true;

            // Add lever colour to the code input
            Room.Input.Add(this.colour);
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
