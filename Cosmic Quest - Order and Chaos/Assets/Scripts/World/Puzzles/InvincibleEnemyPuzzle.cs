using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEnemyPuzzle : EnemyPuzzle
{
    private int invincibleDefense = 100;
    private int defaultDefense = 1;

    private void Start()
    {
        numEnemiesDead = 0;
        numEnemies = enemies.Length;
        foreach(Enemy enemy in enemies)
        {
            EnemyStatsController enemyStats = enemy.enemyPrefab.GetComponent<EnemyStatsController>();
            enemyStats.defense.BaseValue = invincibleDefense;
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    public void RemoveEnemyInvincibility()
    {
        foreach(Enemy enemy in enemies)
        {
            EnemyStatsController enemyStats = enemy.enemyPrefab.GetComponent<EnemyStatsController>();
            enemyStats.defense.BaseValue = defaultDefense;
        }
    }
}
