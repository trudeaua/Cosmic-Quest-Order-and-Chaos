using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerProjectile : Projectile
{
    protected override void OnCollision(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!");
            EnemyStatsController enemy = col.gameObject.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, LauncherStats.ComputeDamageModifer()); // TODO may need to calculate damage differently?
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
            PlayerStatsController player = col.gameObject.GetComponent<PlayerStatsController>();
            player.health.Add(Random.Range(5, 10)); // TODO may need to calculate health differently?
        }
        
        gameObject.SetActive(false);
    }
}
