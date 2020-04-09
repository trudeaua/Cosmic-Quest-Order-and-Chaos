using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverPuzzle : Puzzle
{
    [Tooltip("List of levers involved with this puzzle")]
    public Lever[] levers;
    
    /// <summary>
    /// List of colours required
    /// </summary>
    [SerializeField] protected CharacterColour[] requiredColours;

    /// <summary>
    /// Buffer for received colours
    /// </summary>
    protected List<CharacterColour> Received;

    protected override void Start()
    {      
        base.Start();

        Received = new List<CharacterColour>();

        foreach (Lever lever in levers)
        {
            // Subscribe to lever activation events
            lever.onActivation += AddColour;
        }
    }

    /// <summary>
    /// Add a colour to the received colours buffer
    /// </summary>
    /// <param name="colour">Colour to add to the buffer</param>
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
