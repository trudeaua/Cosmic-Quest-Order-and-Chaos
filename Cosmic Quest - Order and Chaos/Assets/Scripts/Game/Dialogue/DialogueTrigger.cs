using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public void TriggerDialogue ()
    {
        // false to disable interactable dialogue
        DialogueManager.Instance.StartDialogue(dialogue, false); 
    }
}
