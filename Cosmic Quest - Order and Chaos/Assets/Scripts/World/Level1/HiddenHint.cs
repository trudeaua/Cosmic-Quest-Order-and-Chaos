using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenHint : Room
{
    public AudioSource AudioClip;

    private void Awake()
    {
        AudioClip = GetComponent<AudioSource>();
    }
    
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
        AudioClip.Play(0);
        Anim.SetTrigger("RevealHint");
        yield break;
    }
}
