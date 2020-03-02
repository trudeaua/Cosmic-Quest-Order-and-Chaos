using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealerProjectile : Projectile
{
    public GameObject FloatingText;
    private float _damageAmount;
    private float _healAmount; 
    
    public void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range, float damageAmount, float healAmount)
    {
        // Store the damage amount and call the base launch function
        _damageAmount = damageAmount;
        _healAmount = healAmount;
        Launch(launcherStats, direction, launchForce, range);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        if (col.CompareTag("Enemy"))
        {
            EnemyStatsController enemy = col.GetComponent<EnemyStatsController>();
            enemy.TakeDamage(LauncherStats, _damageAmount);
        }
        else if (col.CompareTag("Player"))
        {
            PlayerStatsController player = col.GetComponent<PlayerStatsController>();

            ShowHealingText(_healAmount, player.transform);
            player.health.Add(_healAmount);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Show the amount a player was healed by
    /// </summary>
    /// <param name="healing">Healing amount to display</param>
    /// <param name="transform">Transform to show the healing text at</param>
    /// <param name="duration">Number of seconds to show the healing text for</param>
    /// <returns></returns>
    public void ShowHealingText(float healing, Transform transform, float duration = 0.5f)
    {
        Vector3 offset = new Vector3(0, 5f, 0); // Want to do this dynamically based off enemy height
        float x = 1f, y = 0.5f;
        Vector3 random = new Vector3(Random.Range(-x, x), Random.Range(-y, y));
        GameObject text = Instantiate(FloatingText, transform.position + offset + random, Quaternion.identity, transform);
        text.GetComponent<TMP_Text>().text = healing.ToString("F1");
        Destroy(text, duration);
    }
}
