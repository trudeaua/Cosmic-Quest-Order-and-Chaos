using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for player interactable objects
/// </summary>
public class Draggable : Interactable
{

    private bool isHeld = false;

    public override void Interact(Transform target)
    {
        // Overriding the interact method from base class
        if (CanInteract(target))
        {
            if (isHeld)
            {
                this.Dropped();
            }
            else
            {
                this.PickedUp(target);
            }
        }
    }

    public virtual void Dropped()
    {
        Debug.Log("Dropped"); //todo, actually implement the 'held' mechanic
        this.isHeld = false;
        
           
        this.transform.parent = null; // unparent so it doesn't follow anymore
        GetComponent<Rigidbody>().useGravity = true; //allow it to drop
        GetComponent<Rigidbody>().freezeRotation = false; // allow to spin again
        GetComponent<BoxCollider>().enabled = true;
        
    }

    public virtual void PickedUp(Transform target)
    {
        Debug.Log("Picked up by " + target.name);
        this.isHeld = true;
        
        GetComponent<Rigidbody>().useGravity = false; //stops it from falling
        GetComponent<Rigidbody>().freezeRotation = true; //stops rotation while held
        GetComponent<BoxCollider>().enabled = false;
        //this.transform.position = target.position; // optional, if we want it to SNAP to destination
        this.transform.parent = target.transform; //making the target the PARENT of this object means it will move with it.
         
    }
}
