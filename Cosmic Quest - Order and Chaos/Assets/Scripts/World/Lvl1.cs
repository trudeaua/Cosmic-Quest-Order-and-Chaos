using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1 : Room
{
    private Animator _letterReveal;   // Animation for revealing letter upon room completion
    private AudioSource _letterAudio; // Audio clip for animation

    private void Start()
    {
        Door = transform.Find("Door").gameObject;
        
        Collider = Door.GetComponent<Collider>();
        Anim = Door.GetComponent<Animator>();
        DoorAudio = Door.GetComponent<AudioSource>();

        // Indicates that room has been completed from Progress room perspective
        _letterReveal = transform.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
        _letterAudio = transform.Find("ActivatedLetter").gameObject.GetComponent<AudioSource>();
    }

    public override IEnumerator SetAnimTrigger ()
    {
        yield return new WaitForSeconds(1);

        Anim.SetBool("UnlockDoor", true);
        Collider.enabled = false;

        _letterReveal.SetTrigger("Reveal");

        DoorAudio.Play(0);  
        _letterAudio.Play(0);

        yield break;
    }
}
