using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : Projectile
{
    private float _damage = 0f;
    
    /// <summary>
    /// Launch the damage projectile
    /// </summary>
    /// <param name="launcherStats">Stats of the entity that launched the projectile</param>
    /// <param name="direction">Direction to launch in</param>
    /// <param name="launchForce">Force to apply to the projectile upon launch</param>
    /// <param name="range">Maximum range that the projectile can fly</param>
    /// <param name="damage">Damage of the projectile</param>
    public void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range, float damage)
    {
        // Store the damage amount and call the base launch function
        _damage = damage;
        Launch(launcherStats, direction, launchForce, range);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        // if other object is an enemy, deal damage
        if (col.CompareTag("Enemy"))
        {
            EnemyStatsController enemy = col.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, _damage); // TODO may need to calculate damage differently?
        }
        
        gameObject.SetActive(false);
    }
}
