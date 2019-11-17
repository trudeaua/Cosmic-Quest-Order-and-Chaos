using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        if ((Anim.GetBool("IsGreenActivated")) && 
            (Anim.GetBool("IsPurpleActivated")))
        {
            Anim.SetTrigger("OpenDoor");
        }
    }

    void OnTriggerExit (Collider other)
    {
        Anim.enabled = false;
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
