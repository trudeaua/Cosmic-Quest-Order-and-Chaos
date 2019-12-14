using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom2 : Room
{
    // Update is called once per frame
    void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation for tutorial room 2 once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

}
