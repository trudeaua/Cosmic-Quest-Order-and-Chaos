using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Class for player-lever interaction
public class Lever : Interactable
{
    protected Animator Anim;
    public GameObject Door;

    private void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        Door = transform.parent.parent.Find("Door1").gameObject;

        if (Door == null)
        {
            Debug.Log("no door found");
        }
        else
        {
            Debug.Log("door found");
        }
    }

    private void Reset()
    {
        // Set default value for interactable fields
        isTrigger = true;
    }
    
    public override void StartInteract(Transform target)
    {
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
            Anim.enabled = true;
            Anim.Play("LeverAnimation");
            Anim.SetBool("LeverPulled", true);
            Door.GetComponent<Room>().input.Add(this.colour);
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
