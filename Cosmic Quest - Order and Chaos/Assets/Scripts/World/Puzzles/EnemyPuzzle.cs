using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPuzzle : Puzzle
{
    [Serializable]
    public struct Enemy {
        public GameObject enemyPrefab;
        public CharacterColour enemyColour;
    }
    public Enemy[] enemies;
    [Tooltip("Indicates whether the enemies should attack or not")]
    public bool isAggro = true;
    private List<GameObject> loadedEnemies = new List<GameObject>();
    private int numEnemies;
    private int numEnemiesDead;
    private CharacterColour puzzleColour = CharacterColour.None;

    private void Setup()
    {
        loadedEnemies.Clear();
        numEnemiesDead = 0;
        numEnemies = enemies.Length;
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemyObj = Instantiate(enemies[i].enemyPrefab, transform);
            loadedEnemies.Add(enemyObj);
            EnemyStatsController enemyStats = enemyObj.GetComponent<EnemyStatsController>();
            EnemyBrainController enemyBrain = enemyObj.GetComponent<EnemyBrainController>();
            if (!isAggro)
            {
                enemyBrain.aggroRadius = 0;
            }
            if (puzzleColour != CharacterColour.None && puzzleColour != CharacterColour.All)
            {
                enemyStats.characterColour = puzzleColour;
            }
            else if (playerColours.Contains(enemies[i].enemyColour))
            {
                enemyStats.characterColour = enemies[i].enemyColour;
            }
            else if (enemies[i].enemyColour == CharacterColour.All)
            {
                int colourIndex = UnityEngine.Random.Range(0, playerColours.Length);
                enemyStats.characterColour = playerColours[colourIndex];
            }
            else
            {
                enemyStats.characterColour = CharacterColour.None;
            }
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    public void SetPuzzleColour(CharacterColour colour)
    {
        puzzleColour = colour;
    }

    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
        foreach(GameObject loadedEnemy in loadedEnemies)
        {
            Destroy(loadedEnemy);
        }
        Setup();
    }

    private void EnemyDied()
    {
        numEnemiesDead += 1;
        if (numEnemiesDead == numEnemies)
        {
            // all dead
            SetComplete();
        }
    }
}