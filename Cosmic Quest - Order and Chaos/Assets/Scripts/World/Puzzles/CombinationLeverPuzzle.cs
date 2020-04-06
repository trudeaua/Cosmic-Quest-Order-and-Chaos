using System.Collections.Generic;
using UnityEngine;

public class CombinationLeverPuzzle : LeverPuzzle
{
    // The expected order of colours
    public CharacterColour[] combination;

    protected override void Start()
    {
        activeColours = PlayerManager.Instance.GetActivePlayerColours();

        int numPlayers = PlayerManager.Instance.NumPlayers;
        levers = new Lever[numPlayers];
        Lever[] childLevers = GetComponentsInChildren<Lever>();

        for (int i = 0; i < numPlayers; i++)
        {
            levers[i] = childLevers[i];
        }

        Received = new List<CharacterColour>();
        
        // Subscribe to lever activation events
        foreach (Lever lever in levers)
        {
            lever.onActivation += AddColour;
        }

        SetColourCombination();
    }

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
    /// Set the colour combination of the puzzle based on active player colours
    /// </summary>
    public void SetColourCombination()
    {
        Debug.Log("This is being called");
        for (int i = 0; i < combination.Length; i++)
        {
            combination[i] = activeColours[Random.Range(0, activeColours.Length)];
        }
    }
}
