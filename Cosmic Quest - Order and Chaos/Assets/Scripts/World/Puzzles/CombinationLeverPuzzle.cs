using System.Collections.Generic;
using UnityEngine;

public class CombinationLeverPuzzle : LeverPuzzle
{
    // The expected order of colours
    [SerializeField] private CharacterColour[] combination;

    protected override void AddColour(CharacterColour colour)
    {
        Received.Add(colour);

        // If we have the correct number of elements in buffer then check combination
        if (Received.Count == combination.Length)
        {
            for (int i = 0; i < combination.Length; i++)
            {
                // Incorrect combination
                if (Received[i] != combination[i])
                {
                    // Clear the received buffer
                    Received.Clear();
                    // Play a failure sound?
                    return;
                }
            }
            
            // Combination was correct
            SetComplete();
        }
    }

    /// <summary>
    /// Set the combination of the puzzle based on active player colours
    /// </summary>
    /// <returns>The combination array</returns>
    public CharacterColour[] SetColourCombination()
    {
        CharacterColour[] activeColours = PlayerManager.Instance.GetActivePlayerColours();
        for (int i = 0; i < combination.Length; i++)
        {
            combination[i] = activeColours[Random.Range(0, activeColours.Length)];
        }
        return combination;
    }
}
