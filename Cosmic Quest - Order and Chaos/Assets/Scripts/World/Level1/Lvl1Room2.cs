using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room2 : Room
{
    protected AudioSource audioClip;
    public Animator letterAnim;

    private void Start()
    {
        GameObject letter = transform.Find("ActivatedLetter").gameObject;
        audioClip = letter.GetComponent<AudioSource>();
        letterAnim = letter.GetComponent<Animator>();
    }

    void Update()
    {
        if (AreAllEnemiesKilled())
        {
            letterAnim.SetTrigger("Reveal");
        }
    }
}
