using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    public Animator Anim;
    public CharacterColour colour = CharacterColour.None;
    public bool IsActivated;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void OnTriggerEnter (Collider other) 
    {
        if ((other.tag == "Rock") &&
            (other.gameObject.GetComponent<Interactable>().colour == colour))
        {
            Anim.SetTrigger("PlatformActivated");
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
