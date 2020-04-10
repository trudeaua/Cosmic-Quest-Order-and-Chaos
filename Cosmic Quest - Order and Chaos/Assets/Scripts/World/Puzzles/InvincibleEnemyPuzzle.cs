using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEnemyPuzzle : EnemyPuzzle
{
    private int invincibleDefense = 100;
    private int defaultDefense = 1;

    protected override void Start()
    {
        numEnemiesDead = 0;
        numEnemies = enemyPrefabs.Length;
        foreach(GameObject enemy in enemyPrefabs)
        {
            EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();
            enemyStats.defense.BaseValue = invincibleDefense;
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    public void RemoveEnemyInvincibility()
    {
        foreach(GameObject enemy in enemyPrefabs)
        {
            EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();
            enemyStats.defense.BaseValue = defaultDefense;
        }
    }
}
