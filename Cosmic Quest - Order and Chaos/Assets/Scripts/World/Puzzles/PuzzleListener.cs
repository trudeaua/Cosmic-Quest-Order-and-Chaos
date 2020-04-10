using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleListener : MonoBehaviour
{
    [Tooltip("Puzzles needing completion for Boss battle")]
    public Puzzle[] Puzzles;
    public GameObject bossDoor; 
    public int numCompleted;

    public void IncrementCompletedPuzzles()
    {
        numCompleted += 1;
        if (numCompleted == Puzzles.Length)
        {
            bossDoor.SetActive(true);
        }
    }
}
