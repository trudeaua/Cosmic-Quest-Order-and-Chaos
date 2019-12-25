using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollisionTrigger : MonoBehaviour
{
    public DialogueTrigger Dialogue;
    private Collider _collider;

    private void Awake()
    {
        Dialogue = GetComponent<DialogueTrigger>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        Dialogue.TriggerDialogue();
        
        // Dialogue should only have a lifetime up to the point of it being triggered.
        // Disable this instance to prevent redundant dialogue triggering.
        enabled = false;
    } 
}
