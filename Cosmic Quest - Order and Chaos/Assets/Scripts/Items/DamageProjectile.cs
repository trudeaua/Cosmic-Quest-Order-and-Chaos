using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : Projectile
{
    private float _damage = 0f;
    
    public void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range, float damage)
    {
        // Store the damage amount and call the base launch function
        _damage = damage;
        Launch(launcherStats, direction, launchForce, range);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        if (col.CompareTag("Enemy"))
        {
            EnemyStatsController enemy = col.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, _damage); // TODO may need to calculate damage differently?
        }
        
        gameObject.SetActive(false);
    }
}
