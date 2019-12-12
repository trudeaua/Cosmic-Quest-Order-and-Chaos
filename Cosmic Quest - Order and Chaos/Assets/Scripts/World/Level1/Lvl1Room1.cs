using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Lvl1Room1 : Room
{
    private StringBuilder code;
    private StringBuilder input;

    // Start is called before the first frame update
    void Awake()
    {
        code = new StringBuilder("PGGP", 4);
        input = new StringBuilder("", 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

    // Returns whether all levers in the room have been pulled
    public override bool AreLeversPulled ()
    {
        bool leversPulled = true;

         if (m_Levers == null || m_Levers.Length == 0) return true;

        // Check if every lever has been activated
        foreach (GameObject lever in m_Levers)
        {
            Transform handle = lever.transform.Find("Handle");
            
            if (!handle.GetComponent<Animator>().GetBool("LeverPulled"))
            {
                // If at least 1 lever isn't activated, return false
                leversPulled = false;
            }
        }

        return leversPulled;
    }
}
