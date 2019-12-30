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

    /// <summary>
    /// Display the dialogue
    /// </summary>
    /// <param name="time">Number of seconds to wait before showing the dialogue</param>
    /// <returns>An IEnumerator</returns>
    protected IEnumerator DisplayDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        _dialogue.TriggerDialogue();
        yield break;
    }
}
