using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealerProjectile : Projectile
{
    public GameObject FloatingText;

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
            int healAmount = Random.Range(5, 10);

            ShowHealing(healAmount, player.transform);
            player.health.Add(healAmount); // TODO may need to calculate health differently?
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowHealing(float healing, Transform transform, float duration = 0.5f)
    {
        Vector3 offset = new Vector3(0, 5f, 0); // Want to do this dynamically based off enemy height
        float x = 1f, y = 0.5f;
        Vector3 random = new Vector3(Random.Range(-x, x), Random.Range(-y, y));
        GameObject text = Instantiate(FloatingText, transform.position + offset + random, Quaternion.identity, transform);
        text.GetComponent<TMP_Text>().text = healing.ToString("F1");
        Destroy(text, duration);
    }
}
