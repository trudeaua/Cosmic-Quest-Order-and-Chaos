using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject PortalExit;   // Set exit portal 

    void OnTriggerEnter(Collider other)
    {   
        // Upon colliding with portal, set destination of object to exiting portal's position
        other.transform.position = PortalExit.transform.position;
    }

}
