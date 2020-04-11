using System.Collections;
using System.Linq;
using UnityEngine;

public class RhakCombatController : EnemyCombatController
{
    private Collider _physicalCollider;
    
    [Header("Bullet Hell")]
    public float projectileMinDamage = 20f;
    public float projectileMaxDamage = 30f;
    public float minProjectileForce = 200f;
    public float maxProjectileForce = 450f;
    public float projectileAttackAngle = 30f;
    public int maxProjectilesBeforeOrb = 30;
    public GameObject projectilePrefab;
    [Range(0, 1)] public float orbProbability = 0.1f;
    [Range(0, 1)] public float doubleShotProbability = 0.15f;
    [Range(0, 1)] public float tripleShotProbability = 0.05f;

    public float orbMinDamage = 10f;
    public float orbMaxDamage = 30f;
    public float orbForce = 150f;
    public float orbRange = 15f;
    public GameObject orbPrefab;
    public float minProjectilePeriod = 0.1f;
    public float maxProjectilePeriod = 0.5f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    private bool _isFiring;

    [Header("Charge Attacks")]
    public float cooldownMinTime = 3f;
    public float cooldownMaxTime = 5f;
    public float chargeMinDamage = 10f;
    public float chargeMaxDamage = 20f;
    public float chargeSpeed = 4f;
    public float chargeAcceleration = 2f;
    public int maxConsecutiveCharges = 3;
    
    private Vector3 _chargeTarget;
    private float _currentVelocity;
    private int currentNumProjectiles = 0;
    public bool IsCharging { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // Grab the collider that is not a trigger
        _physicalCollider = GetComponents<Collider>().First(c => !c.isTrigger);
    }

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
            if (target is null)
            {
                // If we don't have a target, then all players must be dead
                _isFiring = false;
                break;
            }

            Vector3 launchDir = target.position - transform.position;

            if (Random.Range(0f, 1f) < orbProbability || ++currentNumProjectiles >= maxProjectilesBeforeOrb)
            {
                // Shoot coloured orb
                float damage = Random.Range(orbMinDamage, orbMaxDamage) + Stats.damage.GetValue();
                LaunchDamageProjectile(orbPrefab, launchDir, orbForce, orbRange, damage, "Player");
                currentNumProjectiles = 0;
            }
            else
            {
                // Shoot projectile
                // Select random direction to launch in
                float angle = Random.Range(-projectileAttackAngle, projectileAttackAngle) / 2;
                float projectileForce = Random.Range(minProjectileForce, maxProjectileForce);
                launchDir = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                float damage = Random.Range(projectileMinDamage, projectileMaxDamage) + Stats.damage.GetValue();
                LaunchDamageProjectile(projectilePrefab, launchDir, projectileForce, 30f, damage, "Player");
                if (Random.Range(0f, 1f) < tripleShotProbability)
                {
                    // :)
                    LaunchDamageProjectile(projectilePrefab, Quaternion.AngleAxis(120f, Vector3.up) * launchDir, projectileForce, 30f, damage, "Player");
                    LaunchDamageProjectile(projectilePrefab, Quaternion.AngleAxis(240f, Vector3.up) * launchDir, projectileForce, 30f, damage, "Player");
                }
                else if (Random.Range(0f, 1f) < doubleShotProbability)
                {
                    LaunchDamageProjectile(projectilePrefab, Quaternion.AngleAxis(180f, Vector3.up) * launchDir, projectileForce, 30f, damage, "Player");
                }
            }
            
            yield return new WaitForSeconds(Random.Range(minProjectilePeriod, maxProjectilePeriod));
        }
    }

    public void StartCharge(Vector3 target)
    {
        IsCharging = true;
        _chargeTarget = target;
        IgnoreCollisionsWithPlayers(true);
        StartCoroutine(ChargeAtTarget());
    }

    private void StopCharge()
    {
        IsCharging = false;
        IgnoreCollisionsWithPlayers(false);
    }

    /// <summary>
    /// Enables or disables physical collisions between the players and Rhak
    /// </summary>
    /// <param name="ignore">Whether to ignore collisions or not</param>
    private void IgnoreCollisionsWithPlayers(bool ignore)
    {
        foreach (GameObject player in PlayerManager.Instance.AlivePlayers)
        {
            // Disable or enable collisions between the player and boss
            Physics.IgnoreCollision(player.GetComponent<Collider>(), _physicalCollider, ignore);
        }
    }

    private IEnumerator ChargeAtTarget()
    {
        Vector3 direction = (_chargeTarget - transform.position).normalized;
        direction.y = 0;
        bool havePassedTarget = false;
        
        while (IsCharging)
        {
            yield return new WaitForFixedUpdate();

            // Calculate the current velocity
            if (havePassedTarget)
            {
                // Decelerate
                if (_currentVelocity > 0)
                    _currentVelocity -= chargeAcceleration * Time.fixedDeltaTime;
                else
                {
                    _currentVelocity = 0;
                    
                    // Stop the charge attack
                    StopCharge();
                }
            }
            else
            {
                // Accelerate
                if (_currentVelocity < chargeSpeed)
                    _currentVelocity += chargeAcceleration * Time.fixedDeltaTime;
                else
                    _currentVelocity = chargeSpeed;
            }

            // Move the rigidbody
            if (_currentVelocity > 0)
            {
                transform.position += direction * _currentVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 6f * Time.fixedDeltaTime);

                if (!havePassedTarget && (transform.position - _chargeTarget).sqrMagnitude <= _currentVelocity * _currentVelocity)
                {
                    havePassedTarget = true;
                }
            }
        }
    }

    /// <summary>
    /// Event that handles dealing damage to the player and allowing the boss to
    /// pass through the player without any collisions
    /// </summary>
    /// <param name="other">The collider we have collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!IsCharging || !other.CompareTag("Player"))
            return;

        // Damage the player
        float damage = Random.Range(chargeMinDamage, chargeMaxDamage) + Stats.damage.GetValue();
        other.GetComponent<EntityStatsController>().TakeDamage(damage);
    }
}
