using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverRockTask : Task
{
    protected override void Start()
    {   
        CombinationLeverPuzzle comboLeverPuzzle = GetComponent<CombinationLeverPuzzle>();
        RockPuzzle rockPuzzle = GetComponent<RockPuzzle>();
        doors = GetComponentsInChildren<Door>();

        // Set and return a colour combination for levers
        CharacterColour[] combination = comboLeverPuzzle.SetColourCombination();

        // Set colours of all interactables in the task based on combination
        for (int i = 0; i < combination.Length; i++)
        {
            rockPuzzle.rocks[i].colour = combination[i];
            rockPuzzle.platforms[i].colour = combination[i];
        }
        introDialogueTrigger.TriggerDialogue();
    }
}
