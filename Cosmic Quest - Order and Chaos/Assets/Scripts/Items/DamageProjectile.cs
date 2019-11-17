using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : Projectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        if (col.CompareTag("Enemy"))
        {
            EnemyStatsController enemy = col.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, LauncherStats.ComputeDamageModifer()); // TODO may need to calculate damage differently?
        }
        
        gameObject.SetActive(false);
    }
}
