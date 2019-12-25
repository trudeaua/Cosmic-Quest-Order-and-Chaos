using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDialogue : OpenDialogue
{
    public Animator DoorAnim;

    void Start()
    {
        _dialogue = GetComponent<DialogueTrigger>();
        DoorAnim = GetComponent<Animator>();
    }
}
