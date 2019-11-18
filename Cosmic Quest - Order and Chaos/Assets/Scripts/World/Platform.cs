using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : Interactable
{
    protected Animator Anim;
    protected GameObject[] Rocks;

    // Start is called before the first frame update
    void Start()
    {
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

    public override bool CanInteract(Transform target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= radius &&
               (target.gameObject.GetComponent<Interactable>().colour == colour);
    }
}
