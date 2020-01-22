using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : Puzzle
{
    // List of colours required
    [SerializeField] private CharacterColour[] requiredColours;
    
    // Buffer for received colours
    protected List<CharacterColour> _received;
    
    private void Start()
    {
        _received = new List<CharacterColour>();
    }

    public void AddColour(CharacterColour c)
    {
        // Don't store duplicates of the same colour entry
        if (!_received.Contains(c))
            _received.Add(c);

        if (_received.Count == requiredColours.Length)
        {
            // All required levers have been pulled
            SetComplete();
        }
    }
}
