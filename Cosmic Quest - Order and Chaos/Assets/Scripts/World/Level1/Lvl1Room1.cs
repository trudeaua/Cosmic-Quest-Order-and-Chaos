using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Lvl1Room1 : Room
{
    public string code;
    public StringBuilder input;

    // Start is called before the first frame update
    void Awake()
    {
        code = "PGGP";
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
         if (input.Length == 4) input.Clear();

        // Check if every lever has been activated
        foreach (GameObject lever in m_Levers)
        {
            Transform handle = lever.transform.Find("Handle");
            
            if (handle.GetComponent<Animator>().GetBool("LeverPulled"))
            {
                switch(handle.GetComponent<Lever>().colour)
                {
                    case (CharacterColour.Green):
                        input.Append('G');
                        break;
                    case (CharacterColour.Purple):
                        input.Append('P');
                        break;
                }
            }
        }

        return (input.ToString() == code);
    }
}
