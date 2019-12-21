using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room3 : Room
{
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
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
