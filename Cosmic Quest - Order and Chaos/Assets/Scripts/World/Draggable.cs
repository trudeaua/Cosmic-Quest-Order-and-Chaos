using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for player interactable objects
/// </summary>
public class Draggable : Interactable
{
    public bool IsPickedUp { get; protected set; }
    
    protected Rigidbody m_Object;
    protected Collider m_Collider;

    protected virtual void Awake ()
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
        if (IsPickedUp)
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
        m_Object.isKinematic = false;
        m_Object.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position.Set(target.position.x, target.position.y, target.position.z + 20);
        IsPickedUp = false;
    }

    /// <summary>
    /// Pick up an object
    /// </summary>
    /// <param name="target">Target that is picking up the object</param>
    public virtual void PickedUp(Transform target)
    {
        Debug.Log("Picked up by " + target.name);
        m_Object.freezeRotation = true; //stops rotation while held
        m_Object.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = target.transform; //making the target the PARENT of this object means it will move with it.
        m_Object.isKinematic = true; //stops it from falling
        transform.localPosition = new Vector3(0, 2.2f, m_Collider.bounds.extents.z + 0.65f);
        IsPickedUp = true;
    }
}
