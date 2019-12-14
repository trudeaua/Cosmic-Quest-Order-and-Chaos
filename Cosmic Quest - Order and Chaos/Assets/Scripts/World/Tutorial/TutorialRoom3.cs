using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom3 : Room
{
    // Update is called once per frame
    void Update()
    {
        if (AreAllEnemiesKilled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation for tutorial room 3 once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }
}
