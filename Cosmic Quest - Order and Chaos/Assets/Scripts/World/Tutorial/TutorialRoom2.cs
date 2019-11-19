using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom2 : Room
{
    protected GameObject[] Levers;

    // Start is called before the first frame update
    void Start()
    {
        Levers = GameObject.FindGameObjectsWithTag("Lever");
        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AreLeversActivated())
        {
            Debug.Log("All levers activated - Open the door.");
            Anim.SetTrigger("OpenDoor");
            m_Collider.enabled = false;

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

    public override bool AreLeversActivated ()
    {
        bool leversActivated = true;

        if (Levers != null)
        {
            foreach (GameObject lever in Levers)
            {
                Transform handle = lever.transform.Find("Handle");
                
                if (!handle.GetComponent<Animator>().GetBool("LeverActivated"))
                {
                    leversActivated = false;
                }
            }
        }

        return leversActivated;
    }

}
