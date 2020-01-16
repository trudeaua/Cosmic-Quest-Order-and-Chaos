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

    /// <summary>
    /// Start interacting with the object
    /// </summary>
    /// <param name="target">Target that is trying to interact with the object</param>
    /// <returns></returns>
    public override void StartInteract(Transform target)
    {
        Debug.Log(CanInteract(target));
        // Overriding the interact method from base class
        if (CanInteract(target))
        {
            PickedUp(target);
        }
    }

    /// <summary>
    /// Stop interacting with the object
    /// </summary>
    /// <param name="target">Target that is interacting with the object</param>
    public override void StopInteract(Transform target)
    {
        // Drop the object
        if (CanInteract(target))
        {
            Dropped(target);
        }
    }

    /// <summary>
    /// Drop the object
    /// </summary>
    public virtual void Dropped(Transform target)
    {
        Debug.Log("Dropped");
        transform.parent = null; // unparent so it doesn't follow anymore
        m_Object.useGravity = true; //allow it to drop
        m_Object.isKinematic = false;
        m_Object.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position.Set(target.position.x, target.position.y, target.position.z + 20);
        m_Collider.enabled = true;
    }

    /// <summary>
    /// Pick up an object
    /// </summary>
    /// <param name="target">Target that is picking up the object</param>
    public virtual void PickedUp(Transform target)
    {
        Debug.Log("Picked up by " + target.name);

        m_Object.useGravity = false; //stops it from falling
        m_Object.isKinematic = true; //stops it from falling
        m_Object.freezeRotation = true; //stops rotation while held
        m_Object.constraints = RigidbodyConstraints.FreezeAll;
        //this.transform.position = target.position; // optional, if we want it to SNAP to destination
        transform.parent = target.transform; //making the target the PARENT of this object means it will move with it.
        transform.localPosition = new Vector3(0, 2.2f, m_Collider.bounds.extents.z);
        // must come after previous line bc it will set the z-position to 0 if collider is not enabled
        m_Collider.enabled = false;
         
    }
}
