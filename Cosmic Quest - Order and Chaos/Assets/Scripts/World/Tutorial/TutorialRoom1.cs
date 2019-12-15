using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom1 : Room
{
    // Update is called once per frame
    void Update()
    {
        if (ArePlatformsActivated())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation for tutorial room 1 once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

}
