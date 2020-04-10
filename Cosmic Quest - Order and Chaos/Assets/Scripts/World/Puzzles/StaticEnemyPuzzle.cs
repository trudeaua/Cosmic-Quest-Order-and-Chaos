using UnityEngine;

/// <summary>
/// Puzzle variant that spawns enemies and completes when the enemies are killed
/// </summary>
public class StaticEnemyPuzzle : EnemyPuzzle
{
    [Tooltip("Scene objects of an enemies that belong to the puzzle")]
    [SerializeField] private GameObject[] enemies;
    protected override void Start()
    {
        Setup();
    }
    /// <summary>
    /// Set up the puzzle
    /// </summary>
    protected override void Setup()
    {
        numEnemiesDead = 0;
        numEnemies = enemies.Length;
        for (int i = 0; i < numEnemies; i++)
        {
            // add any modifieres to the enemy
            EnemyStatsController enemyStats = enemies[i].GetComponent<EnemyStatsController>();
            enemyStats.damage.AddModifier(damageModifier);
            enemyStats.damage.AddModifier(defenseModifier);

            enemyStats.characterColour = CharacterColour.None;
            enemyStats.onDeath.AddListener(EnemyDied);
        }
        if (isBoss)
        {
            GameManager.Instance.SetBossState();
        }
    }

    /// <summary>
    /// Reset the puzzle
    /// </summary>
    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
        Setup();
    }
}