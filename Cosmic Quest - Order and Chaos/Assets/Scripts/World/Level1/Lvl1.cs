using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1 : Room
{
    public Animator LetterReveal;   // Animation for revealing letter upon room completion
    public AudioSource LetterAudio; // Audio clip for animation

    public AnimatorOverrideController AnimationOverride;

    private void Start()
    {
        Door = transform.Find("Door").gameObject;
        
        Collider = Door.GetComponent<Collider>();
        Anim = Door.GetComponent<Animator>();
        DoorAudio = Door.GetComponent<AudioSource>();

        LetterReveal = transform.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
        LetterAudio = transform.Find("ActivatedLetter").gameObject.GetComponent<AudioSource>();
    }

    public override IEnumerator SetAnimTrigger ()
    {
        yield return new WaitForSeconds(1);

        Anim.SetTrigger("UnlockDoor");
        Collider.enabled = false;

        LetterReveal.SetTrigger("Reveal");

        DoorAudio.Play(0);  
        LetterAudio.Play(0);

        yield break;
    }
}
