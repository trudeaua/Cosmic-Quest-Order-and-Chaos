using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Class for player-lever interaction
public class Lever : Interactable
{
    public LeverPuzzle puzzle;
    
    private Animator _anim;
    private AudioSource _audio;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>(); 
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
            _anim.enabled = true;
            _anim.Play("LeverAnimation");
            _anim.SetBool("LeverPulled", true);
            
            _audio.PlayDelayed(0);

            // Add lever colour to the code input
            puzzle.AddColour(colour);
        }
    }

    /// <summary>
    /// Pause the animation of the lever
    /// </summary>
    void PauseAnimationEvent ()
    {
        _anim.enabled = false;
    }
}
