using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPuzzle : Puzzle
{
    public GameObject[] enemies;
    private int numEnemies;
    private int numEnemiesDead;
    // Start is called before the first frame update
    private void Start()
    {
        numEnemiesDead = 0;
        numEnemies = enemies.Length;
        foreach(GameObject enemy in enemies)
        {
            GameObject enemyObj = Instantiate(enemy, transform);
            EnemyStatsController enemyStats = enemyObj.GetComponent<EnemyStatsController>();
            enemyStats.onDeath.AddListener(EnemyDied);
        }
    }

    protected override void Reset()
    {
        base.Reset();
        Start();
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