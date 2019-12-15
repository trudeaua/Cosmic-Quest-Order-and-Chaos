using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Lvl1Room1 : Room
{   
    // Start is called before the first frame update
    void Start()
    {
        // TODO: Implement random generator for lever code patterns based on input of code length and active player colours
        code = new List<CharacterColour>(new CharacterColour[] {CharacterColour.Purple, CharacterColour.Green, CharacterColour.Green, CharacterColour.Purple});
        input = new List<CharacterColour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    // Returns whether all levers in the room have been pulled
    public override bool AreLeversPulled()
    {
        // Clear input on failed tries
        if (input.Count > code.Count) input.Clear();
        if (input.Count != code.Count) return false;

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != code[i]) 
                return false;
        }

        return true;
    }   

}
