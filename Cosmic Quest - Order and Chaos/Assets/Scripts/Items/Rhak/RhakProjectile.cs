using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhakProjectile : DamageProjectile
{
    public float minTimeToSplit = 1.5f;
    public float maxTimeToSplit = 5f;
    public GameObject explosionPrefab;
    public override void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range, float damage, string targetTag = "Enemy")
    {
        ObjectPooler.Instance.StartCoroutine(timedExplosion()); // Just need something that always exists, don't know whats a good choice
        base.Launch(launcherStats, direction, launchForce, range, damage, targetTag);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        // if other object is an enemy, deal damage
        if (col.CompareTag(TargetTag))
        {
            EntityStatsController target = col.GetComponent<EntityStatsController>();
            target.StartCoroutine(other.transform.GetComponent<PlayerMotorController>().ApplyTimedMovementModifier(0.35f, 1.5f));
            target.TakeDamage(LauncherStats, Damage);
        }
        
        // Don't worry about collisions with the launcher or colliders that are triggers
        if (col != LauncherStats.gameObject && !other.isTrigger && other.tag != AllyTag)
        {
            InstantiateExplosion();
            gameObject.SetActive(false);
        }
    }

    IEnumerator timedExplosion()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToSplit, maxTimeToSplit));
        if (gameObject.activeInHierarchy) 
        {
            InstantiateExplosion();
            gameObject.SetActive(false);
        }   
    }

    private void InstantiateExplosion()
    {
        GameObject go = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        RhakProjectileExplosion explosion = go.GetComponentInChildren<RhakProjectileExplosion>();
        explosion.Explode();
    }

    
}
