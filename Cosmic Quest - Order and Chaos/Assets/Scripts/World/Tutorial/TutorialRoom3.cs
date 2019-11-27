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
            StartCoroutine(SetDoorAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }
}
