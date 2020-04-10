using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEnemyPuzzle : EnemyPuzzle
{
    protected override void Start()
    {
        numEnemiesDead = 0;
        numEnemies = enemyPrefabs.Length;
        foreach(GameObject enemy in enemyPrefabs)
        {
            EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();
            enemyStats.invincible = true;
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    public void RemoveEnemyInvincibility()
    {
        foreach(GameObject enemy in enemyPrefabs)
        {
            EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();
            enemyStats.invincible = false;
        }
    }
}
