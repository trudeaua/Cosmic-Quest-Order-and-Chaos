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
            EnemyStatsController enemy = enemyPuzzle.enemyPrefabs[i].GetComponent<EnemyStatsController>();
            enemy.AssignEnemyColour(combination[i]);
        }

        // Hide all platforms
        RockPuzzle rockPuzzle = GetComponent<RockPuzzle>();
        foreach(Platform platform in rockPuzzle.platforms)
        {
            platform.Hide();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTaskArea += 1;
            // once all players are in the task area, the task begins
            if (playersInTaskArea == numPlayers && started == false)
            {
                PlayIntroDialogue();
            }
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