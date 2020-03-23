using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : Projectile
{
    private float _damage = 0f;
    private string _targetTag = "Enemy";

    /// <summary>
    /// Launch the damage projectile
    /// </summary>
    /// <param name="launcherStats">Stats of the entity that launched the projectile</param>
    /// <param name="direction">Direction to launch in</param>
    /// <param name="launchForce">Force to apply to the projectile upon launch</param>
    /// <param name="range">Maximum range that the projectile can fly</param>
    /// <param name="damage">Damage of the projectile</param>
    /// <param name="targetTag">The tag of the target type</param>
    public void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range, float damage, string targetTag = "Enemy")
    {
        // Store the damage amount and call the base launch function
        _damage = damage;
        _targetTag = targetTag;
        Launch(launcherStats, direction, launchForce, range);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        // if other object is an enemy, deal damage
        if (col.CompareTag(_targetTag))
        {
            EnemyStatsController enemy = col.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, _damage);
        }
        
        gameObject.SetActive(false);
    }
}
