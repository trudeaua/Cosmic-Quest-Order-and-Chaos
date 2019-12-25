using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosDoor : MonoBehaviour
{
    protected AudioSource AudioClip;
    protected Animator Anim;

    private void Start()
    {
        AudioClip = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
    }
    
    // Disable door collider if player is passing door. Ensures enemies are contained within rooms.
    void OnTriggerEnter (Collider other) 
    {
        if (other.tag == "Player")
        {
            AudioClip.Play(0);
            Anim.SetTrigger("OpenDoor");
        }
    }

    // Re-enable door collider after player passes door
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Anim.enabled = true;
            Anim.SetTrigger("CloseDoor");
        }
    }
}
