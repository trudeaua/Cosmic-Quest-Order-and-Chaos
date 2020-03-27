using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public void TriggerDialogue ()
    {
        // false to disable interactable dialogue
        DialogueManager.Instance.StartDialogue(dialogue, false); 
    }
}
