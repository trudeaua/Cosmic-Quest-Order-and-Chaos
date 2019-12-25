using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room1 : Lvl1
{   
    public DialogueTrigger Dialogue; // Opening dialogue for Level 1 
    
    private void Start()
    {
        Door = transform.Find("Door").gameObject;
        
        Collider = Door.GetComponent<Collider>();
        Anim = Door.GetComponent<Animator>();
        DoorAudio = Door.GetComponent<AudioSource>();

        // Indicates that room has been completed from Progress room perspective
        LetterReveal = transform.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
        LetterAudio = transform.Find("ActivatedLetter").gameObject.GetComponent<AudioSource>();

        Dialogue = GetComponent<DialogueTrigger>();
        StartCoroutine(DisplayDialogue());
    }

    private void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    // Returns whether all levers in the room have been pulled in correct pattern
    public override bool AreLeversPulled()
    {
        // Clear input on failed tries
        if (Input.Count > Code.Count) Input.Clear();

        // If input count hasn't reached code count, return false
        if (Input.Count != Code.Count) return false;

        for (int i = 0; i < Input.Count; i++)
        {
            if (Input[i] != Code[i]) 
                return false;
        }

        return true;
    }   

    private IEnumerator DisplayDialogue()
    {
        yield return new WaitForSeconds(1);
        Dialogue.TriggerDialogue();
        yield break;
    }
}
