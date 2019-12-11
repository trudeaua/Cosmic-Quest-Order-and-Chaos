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
        transform.parent = null; // unparent so it doesn't follow anymore
        m_Object.useGravity = true; //allow it to drop
        m_Object.constraints = RigidbodyConstraints.None;
        m_Collider.enabled = true;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public virtual void PickedUp(Transform target)
    {
        Debug.Log("Picked up by " + target.name);

        m_Object.useGravity = false; //stops it from falling
        m_Object.freezeRotation = true; //stops rotation while held
        m_Object.constraints = RigidbodyConstraints.FreezeAll;
        //this.transform.position = target.position; // optional, if we want it to SNAP to destination
        transform.parent = target.transform; //making the target the PARENT of this object means it will move with it.
        transform.localPosition = new Vector3(0, 2.2f, m_Collider.bounds.extents.z);
        // must come after previous line bc it will set the z-position to 0 if collider is not enabled
        m_Collider.enabled = false;
         
    }
}
