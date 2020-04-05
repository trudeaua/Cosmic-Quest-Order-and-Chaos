using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverPuzzle : Puzzle
{
    // List of levers involved with this puzzle
    public Lever[] levers;

    // Buffer for received colours
    protected List<CharacterColour> Received;

    protected override void Start()
    {
        Received = new List<CharacterColour>();
        foreach (Lever lever in levers)
        {
            // Subscribe to lever activation events
            lever.onActivation += AddColour;
        }
    }

    protected virtual void AddColour(CharacterColour colour)
    {
        if (isComplete)
            return;

        // Don't store duplicates of the same colour entry
        if (!Received.Contains(colour))
            Received.Add(colour);

        if (Received.Count == playerColours.Length)
        {
            // All required levers have been pulled
            SetComplete();
        }
    }
}
