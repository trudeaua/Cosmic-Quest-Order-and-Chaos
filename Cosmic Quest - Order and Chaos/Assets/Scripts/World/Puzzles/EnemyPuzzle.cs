using System;
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
    private int numEnemies;
    private int numEnemiesDead;

    private void Setup()
    {
        numEnemiesDead = 0;
        numEnemies = enemies.Length;
        foreach (Enemy enemy in enemies)
        {
            GameObject enemyObj = Instantiate(enemy.enemyPrefab, transform);
            EnemyStatsController enemyStats = enemyObj.GetComponent<EnemyStatsController>();

            int colourIndex = UnityEngine.Random.Range(0, playerColours.Length);
            if (playerColours.Contains(enemy.enemyColour))
            {
                enemyStats.characterColour = enemy.enemyColour;
            }
            else
            {
                enemyStats.characterColour = playerColours[colourIndex];
            }
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
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