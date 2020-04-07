using UnityEngine;

public class RockPuzzle : Puzzle
{
    // Platforms required to complete the puzzle
    public Platform[] platforms;
    // Rocks required to complete the puzzle
    public Draggable[] rocks;
    // Current number of activated platforms
    private int _numActivated;
    
    private void Awake()
    {
        platforms = GetComponentsInChildren<Platform>();
        rocks = GetComponentsInChildren<Draggable>();
    }

    private void Start()
    {
        // Subscribe to platform activation events
        foreach (Platform platform in platforms)
        {
            platform.onActivation += UpdateActivated;
        }

        // Randomize colours of interactables
        CharacterColour[] activeColours = PlayerManager.Instance.GetActivePlayerColours();

        if (platforms.Length == rocks.Length)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                CharacterColour colour = activeColours[Random.Range(0, activeColours.Length)];
                platforms[i].colour = colour;
                rocks[i].colour = colour;
            }
        }
        else
        {
            Debug.Log("RockPuzzle: Number of platforms must be equal to number of rocks.");
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
