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
    [Tooltip("Which enemies should be spawned")]
    public Enemy[] enemies;
    [Tooltip("Indicates whether the enemies should attack or not")]
    public bool isAggro = true;
    [Tooltip("Indicates whether the puzzle represents a boss fight or not")]
    public bool isBoss = false;
    [Tooltip("Indicates whether the number of enemies spawned should be auto calculated")]
    public bool autoDetermineNumEnemies = false;
    private int numEnemies;
    private int numEnemiesDead;
    private CharacterColour puzzleColour = CharacterColour.None;
    private List<GameObject> loadedEnemies = new List<GameObject>();

    private void Setup()
    {
        loadedEnemies.Clear();
        numEnemiesDead = 0;
        if (autoDetermineNumEnemies)
        {
            // Base # enemies off number of players + a random number of additional enemies
            numEnemies = playerColours.Length + Mathf.Max(UnityEngine.Random.Range(0, playerColours.Length), UnityEngine.Random.Range(0, enemies.Length));
        }
        else
        {
            numEnemies = enemies.Length;
        }
        for (int i = 0; i < numEnemies; i++)
        {
            int enemyIndex = UnityEngine.Random.Range(0, enemies.Length);
            GameObject enemyObj = Instantiate(enemies[enemyIndex].enemyPrefab, transform);
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
            else if (playerColours.Contains(enemies[enemyIndex].enemyColour))
            {
                enemyStats.characterColour = enemies[enemyIndex].enemyColour;
            }
            else if (enemies[enemyIndex].enemyColour == CharacterColour.All)
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
        if (isBoss)
        {
            GameManager.Instance.SetBossState();
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
        // all dead
        if (numEnemiesDead == numEnemies)
        {
            GameManager.Instance.SetPlayState();
            SetComplete();
        }
    }
}