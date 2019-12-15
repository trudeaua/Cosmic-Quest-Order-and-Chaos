using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    protected Animator Anim;
    public CharacterColour colour = CharacterColour.None;
    public bool IsActivated;

    private AudioSource audioClip;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        audioClip = GetComponent<AudioSource>();
    }

    void OnTriggerEnter (Collider other) 
    {
        if ((other.tag == "Rock") &&
            (other.gameObject.GetComponent<Interactable>().colour == colour))
        {
            Anim.SetTrigger("PlatformActivated");

            audioClip.Play(1);
            IsActivated = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Rock") &&
            (other.gameObject.GetComponent<Interactable>().colour == colour))
        {
            Anim.enabled = true;
            IsActivated = false;
        }
    }

    void PausePlatformAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
