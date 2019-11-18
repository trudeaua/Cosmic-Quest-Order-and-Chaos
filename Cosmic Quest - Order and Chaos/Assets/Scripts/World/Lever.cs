using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for player-lever interaction
public class Lever : Interactable
{
    protected Animator Anim;
    protected GameObject[] Players;

    // Start is called before the first frame update
    void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public override void Interact(Transform target)
    {
        foreach (GameObject player in Players)
        {
            if (CanInteract(target))
            {
                Debug.Log("Interacted with " + target.name);
                Anim.enabled = true;
                Anim.Play("LeverAnimation");
                Anim.SetBool("LeverActivated", true);
            }
        }
       
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
