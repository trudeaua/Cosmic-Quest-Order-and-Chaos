using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom3 : Room
{
    // Update is called once per frame
    void Update()
    {
        if (this.AreAllEnemiesKilled())
        {
            Debug.Log("All enemies killed - Open the door.");
            StartCoroutine(SetDoorAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }
}
