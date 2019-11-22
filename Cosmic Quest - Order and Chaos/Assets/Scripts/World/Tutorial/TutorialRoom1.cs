using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom1 : Room
{
    // Update is called once per frame
    void Update()
    {
        if (this.ArePlatformsActivated())
        {
            Debug.Log("All puzzles solved - Open the door.");
            StartCoroutine(SetDoorAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

}
