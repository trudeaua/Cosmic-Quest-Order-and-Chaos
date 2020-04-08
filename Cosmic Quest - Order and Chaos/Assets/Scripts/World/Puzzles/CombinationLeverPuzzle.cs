using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinationLeverPuzzle : LeverPuzzle
{
    // The expected order of colours
    [SerializeField] private CharacterColour[] combination;

    protected override void Start()
    {
        base.Start();
        foreach (Lever lever in levers)
        {
            if (!playerColours.Contains(lever.colour))
            {
                gameObject.SetActive(false);
                break;
            }
        }
    }

    protected override void AddColour(CharacterColour colour)
    {
        if (isComplete)
            return;

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
}
