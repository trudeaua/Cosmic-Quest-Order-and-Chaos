using System.Collections;
using System.Linq;
using UnityEngine;

public class RhakCombatController : EnemyCombatController
{
    [Header("Bullet Hell")]
    public float projectileMinDamage = 5f;
    public float projectileMaxDamage = 10f;
    public float projectileForce = 300f;
    public GameObject projectilePrefab;
    [Range(0, 1)] public float orbProbability = 0.1f;
    public float orbMinDamage = 10f;
    public float orbMaxDamage = 30f;
    public float orbForce = 150f;
    public GameObject orbPrefab;
    public float projectilePeriod = 0.5f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    private bool _isFiring;

    [Header("Charge Attacks")]
    public float chargeMinDamage = 10f;
    public float chargeMaxDamage = 20f;
    
    public void StartBulletHell()
    {
        _isFiring = true;
        StartCoroutine(BulletHell());
    }

    public void StopBulletHell()
    {
        _isFiring = false;
    }

    private IEnumerator BulletHell()
    {
        while (_isFiring)
        {
            Transform target = Brain.GetCurrentTarget();

            if (Random.Range(0f, 1f) < orbProbability)
            {
                // Shoot coloured orb
                float damage = Random.Range(orbMinDamage, orbMaxDamage) + Stats.damage.GetValue();
                LaunchDamageProjectile(orbPrefab, transform.forward, orbForce, 30f, damage, "Player");
            }
            else
            {
                // Shoot projectile
                float damage = Random.Range(projectileMinDamage, projectileMaxDamage) + Stats.damage.GetValue();
                LaunchDamageProjectile(projectilePrefab, transform.forward, projectileForce, 30f, damage, "Player");
            }
            
            yield return new WaitForSeconds(projectilePeriod);
        }
    }
}
