using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room3 : Room
{
    public Animator letterReveal;
    
    private void Start()
    {
        // TODO: Implement random generator for lever code patterns based on input of code length and active player colours
        code = new List<CharacterColour>();
        input = new List<CharacterColour>();
        letterReveal = transform.parent.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (AreAllEnemiesKilled())
        {

        }
    }

    void OnTriggerEnter (Collider other) 
    {
        if (other.tag == "Player")
        {
            audioClip.Play(0);
            Anim.SetTrigger("OpenDoor");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Anim.enabled = true;
        }
    }
}
