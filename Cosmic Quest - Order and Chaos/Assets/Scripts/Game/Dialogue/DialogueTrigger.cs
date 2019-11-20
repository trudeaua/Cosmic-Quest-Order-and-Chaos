using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue (bool interactable = false)
    {
        DialogueManager.Instance.StartDialogue(dialogue, interactable);
    }
}
