using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoom : Lvl1
{    
    private void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    /// <summary>
    /// Checks if all the levers in the room are pulled
    /// </summary>
    /// <returns>A boolean indicating whether all levers in the room have been pulled in correct pattern or not</returns>
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
}
