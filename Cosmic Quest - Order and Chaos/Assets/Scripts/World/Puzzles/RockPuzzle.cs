using UnityEngine;

public class RockPuzzle : Puzzle
{
    [Tooltip("Platforms in the puzzle")]
    public Platform[] platforms;

    /// <summary>
    /// Current number of activated platforms
    /// </summary>
    private int numActivated;
    
    /// <summary>
    /// Number of platforms required to be activated
    /// </summary>
    private int requiredNumActivations;

    protected override void Start()
    {
        base.Start();
        requiredNumActivations = platforms.Length;
        foreach (Platform platform in platforms)
        {
            // Subscribe to platform activation events
            platform.onActivation += UpdateActivated;

            // If the platform is inactive it's not required
            if (!platform.gameObject.activeInHierarchy)
            {
                requiredNumActivations -= 1;
            }
        }
    }

    /// <summary>
    /// Updates the number of activated platforms
    /// </summary>
    /// <param name="isActivated"></param>
    private void UpdateActivated(bool isActivated)
    {
        if (IsComplete)
            return;

        numActivated += isActivated ? 1 : -1;

        if (numActivated == requiredNumActivations)
        {
            // Puzzle is complete
            SetComplete();
        }
    }
}
