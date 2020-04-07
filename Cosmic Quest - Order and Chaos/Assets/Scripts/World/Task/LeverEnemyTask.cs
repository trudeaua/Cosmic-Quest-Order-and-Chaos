using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverEnemyTask : Task
{
    protected override void Start()
    {   
        CombinationLeverPuzzle combinationPuzzle = GetComponent<CombinationLeverPuzzle>();
        EnemyPuzzle enemyPuzzle = GetComponent<EnemyPuzzle>();

        // Set colours of all enemies in the task based on combination
        CharacterColour[] combination = combinationPuzzle.SetColourCombination();
        for (int i = 0; i < combination.Length; i++)
        {
            EnemyStatsController enemy = enemyPuzzle.enemies[i].enemyPrefab.GetComponent<EnemyStatsController>();
            enemy.AssignEnemyColour(combination[i]);
            enemyPuzzle.enemies[i].enemyColour = combination[i];
        }

        // Hide all platforms
        RockPuzzle rockPuzzle = GetComponent<RockPuzzle>();
        foreach(Platform platform in rockPuzzle.platforms)
        {
            platform.Hide();
        }
    }

    /// <summary>
    /// Unhide all platforms
    /// </summary>
    public void UnhidePlatforms()
    {
        RockPuzzle rockPuzzle = GetComponent<RockPuzzle>();
        foreach(Platform platform in rockPuzzle.platforms)
        {
            platform.Unhide();
        }
    }
}