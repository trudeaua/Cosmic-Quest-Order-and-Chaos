using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for player interactable objects
/// </summary>
public class Draggable : Interactable
{
    protected Rigidbody m_Object;
    protected Collider m_Collider;

    void Awake ()
    {
        m_Object = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    public override void StartInteract(Transform target)
    {
        // Overriding the interact method from base class
        if (CanInteract(target))
        {
            PickedUp(target);
        }
    }

    public override void StopInteract(Transform target)
    {
        // Drop the object
        Dropped();
    }

    public virtual void Dropped()
    {
        Debug.Log("Dropped");

        this.transform.parent = null; // unparent so it doesn't follow anymore
        m_Object.useGravity = true; //allow it to drop
        m_Object.constraints = RigidbodyConstraints.None;
        m_Collider.enabled = true;
        
    }

    public virtual void PickedUp(Transform target)
    {
        Debug.Log("Picked up by " + target.name);

        m_Object.useGravity = false; //stops it from falling
        m_Object.freezeRotation = true; //stops rotation while held
        m_Object.constraints = RigidbodyConstraints.FreezeAll;
        m_Collider.enabled = false;
        //this.transform.position = target.position; // optional, if we want it to SNAP to destination
        this.transform.parent = target.transform; //making the target the PARENT of this object means it will move with it.
         
    }
}
