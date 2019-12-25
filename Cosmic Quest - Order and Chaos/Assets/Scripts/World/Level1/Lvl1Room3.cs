using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room3 : Lvl1
{
    void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    
    // Returns whether all levers in the room have been pulled in correct pattern
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
