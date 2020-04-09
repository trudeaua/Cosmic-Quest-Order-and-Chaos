using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatformPuzzle : RockPuzzle
{
    // enemies required to complete the puzzle
    private EnemyStatsController[] enemies;

    protected override void Awake()
    {
        platforms = GetComponentsInChildren<Platform>();
        enemies = GetComponentsInChildren<EnemyStatsController>();
    }

    protected virtual void Start()
    {
        playerColours = PlayerManager.Instance.CurrentPlayerColours;
        
        // Subscribe to platform activation events
        foreach (Platform platform in platforms)
        {
            platform.onActivation += UpdateActivated;
        }

        // Randomize colours of enemies and platforms
        if (platforms.Length == enemies.Length)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                CharacterColour colour = playerColours[Random.Range(0, playerColours.Length)];
                platforms[i].colour = colour;
                enemies[i].characterColour = colour;
            }
        }
    }
}
