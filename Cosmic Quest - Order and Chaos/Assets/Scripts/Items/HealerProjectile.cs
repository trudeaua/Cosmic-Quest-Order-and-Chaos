using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerProjectile : Projectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        if (col.CompareTag("Enemy"))
        {
            EnemyStatsController enemy = col.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, LauncherStats.ComputeDamageModifer()); // TODO may need to calculate damage differently?
        }
        else if (col.CompareTag("Player"))
        {
            PlayerStatsController player = col.GetComponent<PlayerStatsController>();
            player.health.Add(Random.Range(5, 10)); // TODO may need to calculate health differently?
        }
    }
}
