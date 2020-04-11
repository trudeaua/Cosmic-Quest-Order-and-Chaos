using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhakProjectileExplosion : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float launchForce = 500f;
    public float range = 10f;
    public float minDamage = 5f;
    public float maxDamage = 10f;
    public int minProjectileCount = 2;
    public int maxProjectileCount = 8;

    [SerializeField]
    private EntityStatsController statsController;

    private float duration = 1f;

    public void Explode()
    {
        ObjectPooler.Instance.StartCoroutine(launchProjectiles());
    }

    IEnumerator launchProjectiles()
    {
        yield return new WaitForSeconds(duration);

        int numProjectiles = Random.Range(minProjectileCount, maxProjectileCount);
        Vector3 targetDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        float angleIncrement = 360f / numProjectiles;
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject projectile = ObjectPooler.Instance.GetPooledObject(projectilePrefab);
            projectile.GetComponent<DamageProjectile>().Launch(statsController, Quaternion.AngleAxis(angleIncrement * i, Vector3.up) * targetDirection, launchForce, range, Random.Range(minDamage, maxDamage), "Player");
        }
    }
}
