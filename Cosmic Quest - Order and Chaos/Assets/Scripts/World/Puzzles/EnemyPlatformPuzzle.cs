using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatformPuzzle : RockPuzzle
{
    [Tooltip("Required enemies to complete the puzzle")]
    public EnemyStatsController[] enemies;

    protected override void Awake()
    {
        platforms = GetComponentsInChildren<Platform>();
        enemies = GetComponentsInChildren<EnemyStatsController>();
    }

    protected override void Start()
    {
        playerColours = PlayerManager.Instance.CurrentPlayerColours;
        requiredNumActivations = platforms.Length;

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
                platforms[i].SetMaterial(colour);
                enemies[i].AssignEnemyColour(colour);
            }
        }
    }
}
