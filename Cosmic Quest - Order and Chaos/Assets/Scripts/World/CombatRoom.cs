using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : Lvl1
{
    void Update()
    {
        if (AreAllEnemiesKilled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }
}
