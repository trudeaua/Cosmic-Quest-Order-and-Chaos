using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    protected Animator Anim;
    public CharacterColour Colour = CharacterColour.All;
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
            (other.gameObject.GetComponent<Interactable>().colour == Colour))
        {
            IsActivated = true;
            Anim.SetTrigger("PlatformActivated");

            audioClip.PlayDelayed(0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Rock") &&
            (other.gameObject.GetComponent<Interactable>().colour == Colour))
        {
            IsActivated = false;
            Anim.enabled = true;
        }
    }

    /// <summary>
    /// Pause the platform animation
    /// </summary>
    void PausePlatformAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
