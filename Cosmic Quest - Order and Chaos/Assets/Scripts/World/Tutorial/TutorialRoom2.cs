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
            Debug.Log("All levers pulled - Open the door.");
            StartCoroutine(SetDoorAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

}
