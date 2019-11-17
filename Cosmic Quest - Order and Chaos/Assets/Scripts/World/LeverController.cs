using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : Interactable
{
    protected Transform Lever;
    protected Animator Anim; 

    // Start is called before the first frame update
    void Start()
    {
        Lever = gameObject.transform;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Transform target)
    {
        // This function is intended to be overriden
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
        }
    }

    void OnTriggerEvent (Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Anim.SetTrigger("PullLever");
            Debug.Log("Lever Pulled");
            Anim.Play("leverAnimation");    
        }
    }

    void PauseAnimation ()
    {
        Anim.enabled = false;
    }
}
