using UnityEngine;

public class RockPuzzle : Puzzle
{
    [Tooltip("Platforms in the puzzle")]
    public Platform[] platforms;

    [Tooltip("Rocks in the puzzle")]
    public Draggable[] rocks;

    /// <summary>
    /// Current number of activated platforms
    /// </summary>
    protected int numActivated;
    
    /// <summary>
    /// Number of platforms required to be activated
    /// </summary>
    protected int requiredNumActivations;

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

        // Randomize colours of interactables
        if (platforms.Length == rocks.Length)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                CharacterColour colour = playerColours[Random.Range(0, playerColours.Length)];
                platforms[i].colour = colour;
                rocks[i].colour = colour;
                platforms[i].SetMaterial(colour);
                rocks[i].SetMaterialColour(colour);
            }
        }
    }

    /// <summary>
    /// Updates the number of activated platforms
    /// </summary>
    /// <param name="isActivated"></param>
    protected void UpdateActivated(bool isActivated)
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
