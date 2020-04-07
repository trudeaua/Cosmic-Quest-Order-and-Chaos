using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPuzzle : Puzzle
{
    [Serializable]
    public struct Enemy {
        public GameObject enemyPrefab;
        public CharacterColour enemyColour;
    }
    public Enemy[] enemies;
    protected int numEnemies;
    protected int numEnemiesDead;

    protected virtual void Setup()
    {
        numEnemiesDead = 0;
        numEnemies = enemies.Length;
        foreach(Enemy enemy in enemies)
        {
            GameObject enemyObj = Instantiate(enemy.enemyPrefab, transform);
            EnemyStatsController enemyStats = enemyObj.GetComponent<EnemyStatsController>();
            NavMeshAgent navMesh = enemyObj.GetComponent<NavMeshAgent>();
            navMesh.enabled = false;
            if (enemy.enemyColour == CharacterColour.All)
            {
                enemyStats.characterColour = puzzleColour;
            }
            else
            {
                enemyStats.characterColour = enemy.enemyColour;
            }
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
        Setup();
    }

    protected void EnemyDied()
    {
        numEnemiesDead += 1;
        if (numEnemiesDead == numEnemies)
        {
            // all dead
            SetComplete();
        }
    }
}