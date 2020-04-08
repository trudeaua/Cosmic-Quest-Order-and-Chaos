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

        // Set a colour combination for levers
        comboLeverPuzzle.SetColourCombination();

        // Set colours of all interactables in the task based on combination
        for (int i = 0; i < comboLeverPuzzle.combination.Length; i++)
        {
            rockPuzzle.rocks[i].colour = comboLeverPuzzle.combination[i];
            rockPuzzle.platforms[i].colour = comboLeverPuzzle.combination[i];
        }
        introDialogueTrigger.TriggerDialogue();
    }
}
