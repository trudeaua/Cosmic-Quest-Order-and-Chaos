using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room3 : Room
{
    protected AudioSource audioClip;
    public Animator letterReveal;

    private void Start()
    {
        letterReveal = transform.parent.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (AreAllEnemiesKilled())
        {
            letterReveal.SetTrigger("Reveal");
        }
    }
}
