using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenHint : Room
{
    public Room Room;
    public AudioSource AudioClip;

    private void Awake()
    {
        Room = transform.parent.gameObject.GetComponent<Room>();
        AudioClip = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {   
        // If all platforms in room 1 are activated, reveal hint for players
        if (Room.ArePlatformsActivated())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger hint animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    public override IEnumerator SetAnimTrigger ()
    {
        Anim.SetTrigger("RevealHint");  
        AudioClip.Play(0);
        yield break;
    }
}
