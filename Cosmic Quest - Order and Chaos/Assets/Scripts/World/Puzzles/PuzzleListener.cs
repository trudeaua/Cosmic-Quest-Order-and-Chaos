using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleListener : MonoBehaviour
{
    [Tooltip("Puzzles needing completion for Boss battle")]
    public Puzzle[] Puzzles;
    
    [Tooltip("Dialogue to play after completing all puzzles")]
    public DialogueTrigger dialogueTrigger;

    public GameObject bossDoor; 
    public int numCompleted;



    public void IncrementCompletedPuzzles()
    {
        numCompleted += 1;
        if (numCompleted >= Puzzles.Length)
        {
            bossDoor.SetActive(true);
            dialogueTrigger.TriggerDialogue();
        }
    }
}
