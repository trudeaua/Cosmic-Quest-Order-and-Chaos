using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    public Animator Anim;
    public CharacterColour colour = CharacterColour.None;

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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Rock") &&
            (other.gameObject.GetComponent<Interactable>().colour == colour))
        {
            Anim.enabled = true;
        }
    }

    void PausePlatformAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
