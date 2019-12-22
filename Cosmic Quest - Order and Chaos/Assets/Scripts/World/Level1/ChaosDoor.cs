using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosDoor : MonoBehaviour
{
    public AudioSource audioClip;
    protected Animator Anim;

    void Start()
    {
        audioClip = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
    }

    void OnTriggerEnter (Collider other) 
    {
        if (other.tag == "Player")
        {
            audioClip.Play(0);
            Anim.SetTrigger("OpenDoor");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Anim.enabled = true;
        }
    }
}
