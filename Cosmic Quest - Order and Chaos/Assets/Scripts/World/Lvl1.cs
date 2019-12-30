using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1 : Room
{
    private Animator _letterReveal;   // Animation for revealing letter upon room completion
    private AudioSource _letterAudio; // Audio clip for animation

    public Animator BossDoor;  // Unlocks when all rooms are completed

    private void Start()
    {
        Door = transform.Find("Door").gameObject;
        
        Collider = Door.GetComponent<Collider>();
        Anim = Door.GetComponent<Animator>();
        DoorAudio = Door.GetComponent<AudioSource>();

        // Indicates that room has been completed from Progress room perspective
        _letterReveal = transform.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
        _letterAudio = transform.Find("ActivatedLetter").gameObject.GetComponent<AudioSource>();

        BossDoor = transform.parent.Find("BossDoor").gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// Play the door animation
    /// </summary>
    /// <returns>An IEnumerator</returns>
    public override IEnumerator SetAnimTrigger ()
    {
        yield return new WaitForSeconds(1);

        Anim.SetBool("UnlockDoor", true);
        Collider.enabled = false;

        _letterReveal.SetTrigger("Reveal");

        BossDoor.SetInteger("CompletedRooms", BossDoor.GetInteger("CompletedRooms") + 1);

        DoorAudio.PlayDelayed(0);  
        _letterAudio.PlayDelayed(0);

        yield break;
    }
}
