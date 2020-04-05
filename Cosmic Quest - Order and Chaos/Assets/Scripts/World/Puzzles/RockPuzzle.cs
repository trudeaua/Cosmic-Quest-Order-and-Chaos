using System.Linq;
using UnityEngine;

public class RockPuzzle : Puzzle
{
    // Platforms required to complete the puzzle
    public Platform[] platforms;
    // Current number of activated platforms
    private int _numActivated;
    
    protected override void Start()
    {
        base.Start();
        foreach (Platform platform in platforms)
        {
            // Subscribe to platform activation events
            platform.onActivation += UpdateActivated;
        }
    }

    private void UpdateActivated(bool isActivated)
    {
        if (isComplete)
            return;

        _numActivated += isActivated ? 1 : -1;

        if (_numActivated == puzzleColours.Length)
        {
            // Puzzle is complete
            SetComplete();
        }
    }
}
