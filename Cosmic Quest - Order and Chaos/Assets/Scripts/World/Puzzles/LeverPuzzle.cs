using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : Puzzle
{
    // List of levers involved with this puzzle
    public Lever[] levers;
    // List of colours required
    [SerializeField] protected CharacterColour[] requiredColours;
    // Buffer for received colours
    protected List<CharacterColour> Received;
    
    private void Awake()
    {
        levers = GetComponentsInChildren<Lever>();
    }

    protected void Start()
    {      
        // Remove inactive coloured levers 
        int numActivePlayers = PlayerManager.Instance.NumPlayers;
        int numTotalPlayers = PlayerManager.Instance.PlayerColours.Length;
        for (int i = 0; i < (numTotalPlayers); i++)
        {
            if (i >= numActivePlayers)
            {
                levers[i].gameObject.SetActive(false);
            }
        }

        Received = new List<CharacterColour>();
        
        // Subscribe to lever activation events
        foreach (Lever lever in levers)
        {
            lever.onActivation += AddColour;
        }
    }

    protected virtual void AddColour(CharacterColour colour)
    {
        // Don't store duplicates of the same colour entry
        if (!Received.Contains(colour))
            Received.Add(colour);

        if (Received.Count == requiredColours.Length)
        {
            // All required levers have been pulled
            SetComplete();
        }
    }
}
