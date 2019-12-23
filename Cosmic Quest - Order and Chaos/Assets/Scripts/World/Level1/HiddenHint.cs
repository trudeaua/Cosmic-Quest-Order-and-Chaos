using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenHint : MonoBehaviour
{
    private Room _room;
    private AudioSource _audioClip;
    protected Animator Anim;

    private void Start()
    {
        _room = transform.parent.gameObject.GetComponent<Room>();
        _audioClip = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {   
        // If all platforms in room 1 are activated, reveal hint for players
        if (_room.ArePlatformsActivated())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger hint animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    private IEnumerator SetAnimTrigger ()
    {
        Anim.SetTrigger("RevealHint");  
        _audioClip.Play(0);
        yield break;
    }
}
