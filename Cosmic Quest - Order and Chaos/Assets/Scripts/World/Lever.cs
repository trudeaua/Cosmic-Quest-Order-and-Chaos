using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for player-lever interaction
public class Lever : Interactable
{
    protected Animator Anim;
    protected GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        Player = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact(Player.transform);
        }
    }
    
    public override void Interact(Transform target)
    {
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
            
            Anim.Play("leverAnimation");
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
