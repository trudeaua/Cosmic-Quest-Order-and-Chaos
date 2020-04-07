using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatformPuzzle : RockPuzzle
{
    // enemeis required to complete the puzzle
    public EnemyStatsController[] enemies;

    protected override void Awake()
    {
        platforms = GetComponentsInChildren<Platform>();
        enemies = GetComponentsInChildren<EnemyStatsController>();
    }

    protected virtual void Start()
    {
        // Subscribe to platform activation events
        foreach (Platform platform in platforms)
        {
            platform.onActivation += UpdateActivated;
        }

        // Randomize colours of enemies and platforms
        CharacterColour[] activeColours = PlayerManager.Instance.GetActivePlayerColours();

        if (platforms.Length == enemies.Length)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                CharacterColour colour = activeColours[Random.Range(0, activeColours.Length)];
                platforms[i].colour = colour;
                enemies[i].characterColour = colour;
            }
        }
        else
        {
            Debug.Log("RockPuzzle: Number of platforms must be equal to number of rocks.");
        }
    }
}
