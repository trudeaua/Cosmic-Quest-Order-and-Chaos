using UnityEngine;

public class RockPuzzle : Puzzle
{
    // Platforms required to complete the puzzle
    public Platform[] platforms;
    // Current number of activated platforms
    private int _numActivated;
    
    private void Start()
    {
        // Subscribe to platform activation events
        foreach (Platform platform in platforms)
        {
            platform.onActivation += UpdateActivated;
        }
    }

    private void UpdateActivated(bool isActivated)
    {
        _numActivated += isActivated ? 1 : -1;

        if (_numActivated == platforms.Length)
        {
            // Puzzle is complete
            SetComplete();
        }
    }
}
