using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator m_doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        m_doorAnim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        if ((m_doorAnim.GetBool("IsGreenActivated")) && 
            (m_doorAnim.GetBool("IsPurpleActivated")))
        {
            m_doorAnim.SetTrigger("OpenDoor");
        }
    }

    void OnTriggerExit (Collider other)
    {
        m_doorAnim.enabled = false;
    }

    void PauseAnimationEvent ()
    {
        m_doorAnim.enabled = false;
    }
}
