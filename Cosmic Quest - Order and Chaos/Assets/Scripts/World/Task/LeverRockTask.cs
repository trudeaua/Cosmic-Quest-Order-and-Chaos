using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverRockTask : Task
{
    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();
        _Puzzles = GetComponents<Puzzle>();    
    }

    protected override void Start()
    {   
        CharacterColour[] combination = new CharacterColour[numPlayers];

        foreach (Puzzle puzzle in _Puzzles)
        {
            if (puzzle is CombinationLeverPuzzle)
            {
                CombinationLeverPuzzle comboLeverPuzzle = puzzle as CombinationLeverPuzzle;
                combination = comboLeverPuzzle.SetColourCombination();
            }
        }

        // Set colours of other interactables in the task based on combination
        foreach (Puzzle puzzle in _Puzzles)
        {
            if (puzzle is RockPuzzle)
            {
                RockPuzzle rockPuzzle = puzzle as RockPuzzle;
                for (int i = 0; i < combination.Length; i++)
                {
                    rockPuzzle.rocks[i].colour = combination[i];
                    rockPuzzle.platforms[i].colour = combination[i];
                }
            } 
        }
    }
}
