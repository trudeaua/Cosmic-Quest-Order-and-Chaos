using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDialogue : MonoBehaviour
{
    private DialogueTrigger _dialogue; // Opening dialogue for Level 1 

    void Start()
    {
        _dialogue = GetComponent<DialogueTrigger>();
        StartCoroutine(DisplayDialogue(1));
    }

    protected IEnumerator DisplayDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        _dialogue.TriggerDialogue();
        yield break;
    }
}
