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

            // Only need to trigger hint animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    public override IEnumerator SetAnimTrigger ()
    {
        audioClip.Play(0);
        Anim.SetTrigger("RevealHint");
        yield break;
    }
}
