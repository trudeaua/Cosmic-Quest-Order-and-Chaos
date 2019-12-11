using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenHint : Room
{
    // Update is called once per frame
    void Update()
    {
        if (ArePlatformsActivated())
        {
            StartCoroutine(SetAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

    public override IEnumerator SetAnimTrigger ()
    {
        Anim.SetTrigger("RevealHint");
        yield break;
    }
}
