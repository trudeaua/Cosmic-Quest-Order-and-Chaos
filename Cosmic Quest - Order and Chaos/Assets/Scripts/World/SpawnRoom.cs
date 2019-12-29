using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : Lvl1
{
    private void Update()
    {
        if (ArePlatformsActivated())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }
}
