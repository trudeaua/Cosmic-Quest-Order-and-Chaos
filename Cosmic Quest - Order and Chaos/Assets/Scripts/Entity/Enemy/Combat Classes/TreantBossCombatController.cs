using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreantBossCombatController : EnemyCombatController
{
    [Header("Bite Attack")]
    public float primaryAttackCooldown = 2f;
    public float primaryAttackRadius = 6f;
    public float primaryAttackAngle = 45f;
    public float primaryAttackMinDamage = 10f;
    public float primaryAttackMaxDamage = 20f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Stomp Attack")]
    public float secondaryAttackCooldown = 3f;
    public float secondaryAttackRadius = 7f;
    public float secondaryAttackAngle = 200f;
    public float secondaryAttackMinDamage = 10f;
    public float secondaryAttackMaxDamage = 30f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;

    [Header("Projectile Attack")]
    public float projectileAttackCooldown = 1f;
    public float projectileAttackForce = 1000f;
    public float projectileAttackAngle = 120f;
    public int projectileAttackNumSeeds = 3;
    public float projectileAttackMinDamage = 5f;
    public float projectileAttackMaxDamage = 10f;
    public int maxPlantsSpawned = 10;
    public GameObject seedPrefab;
    [SerializeField] protected AudioHelper.EntityAudioClip projectileAttackSFX;
    private List<EnemyStatsController> _spawnedPlants;
    
    [Header("Special Attack - Roots")]
    public float rootAttackCooldown = 1f;
    public GameObject rootContainerPrefab;
    [SerializeField] protected AudioHelper.EntityAudioClip rootAttackSFX;
    private TreantRootContainer _roots;
    
    [Header("Special Attack - Launch Seeds")]
    public float seedAttackCooldown = 1f;
    public float seedAttackForce = 1000f;
    public float seedAttackAngle = 160f;
    public float seedAttackMinDamage = 5f;
    public float seedAttackMaxDamage = 10f;
    public float seedLaunchPeriod = 0.2f;
    [SerializeField] protected AudioHelper.EntityAudioClip seedAttackSFX;
    private bool _isLaunchingSeeds;

    [Header("Strategy Settings")]
    [Tooltip("Max Distance from the target for the Treant to attempt melee attacks")]
    public float meleeDistance = 7f;
    [Tooltip("Distance from the target for the Treant to attempt ranged attacks")]
    public float rangedDistance = 10f;
    
    protected override void Awake()
    {
        base.Awake();

        GameObject roots = Instantiate(rootContainerPrefab);
        _roots = roots.GetComponent<TreantRootContainer>();
        _spawnedPlants = new List<EnemyStatsController>();
    }

    /// <summary>
    /// Bite attack
    /// </summary>
    public override void PrimaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, primaryAttackSFX));

        // Attack any players within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, primaryAttackRadius, primaryAttackAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(primaryAttackMinDamage, primaryAttackMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
    }

    /// <summary>
    /// Stomp attack
    /// </summary>
    public override void SecondaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, secondaryAttackSFX));

        // Attack any players within the attack range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, secondaryAttackRadius, secondaryAttackAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(secondaryAttackMinDamage, secondaryAttackMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
    }

    /// <summary>
    /// Spit seeds attack
    /// </summary>
    public void ProjectileAttack()
    {
        for (int i = 0; i < projectileAttackNumSeeds; i++)
        {
            // Select random direction to launch in
            float angle = Random.Range(-projectileAttackAngle, projectileAttackAngle) / 2;
            Vector3 launchDir = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
            
            // Launch the seed projectile
            float damageValue = Random.Range(projectileAttackMinDamage, projectileAttackMaxDamage) + Stats.damage.GetValue();
            LaunchDamageProjectile(seedPrefab, launchDir, projectileAttackForce, 20f, damageValue, "Player");
        }
    }
    
    /// <summary>
    /// Event function to start the root attack
    /// </summary>
    public void RootAttack()
    {
        // Move root container to boss
        _roots.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        _roots.StartAttack();
        AttackCooldown = rootAttackCooldown;
    }

    public void StartSeedAttack()
    {
        _isLaunchingSeeds = true;
        StartCoroutine(SeedLaunchAttack());
    }

    public void StopSeedAttack()
    {
        _isLaunchingSeeds = false;
        AttackCooldown = seedAttackCooldown;
    }

    private IEnumerator SeedLaunchAttack()
    {
        while (_isLaunchingSeeds)
        {
            // Select random direction to launch in
            float breadth = Random.Range(-seedAttackAngle, seedAttackAngle) / 2;
            Vector3 launchDir = Quaternion.AngleAxis(breadth, Vector3.up) * transform.forward;
            
            // Launch the seed projectile
            float damageValue = Random.Range(seedAttackMinDamage, seedAttackMaxDamage) + Stats.damage.GetValue();
            LaunchDamageProjectile(seedPrefab, launchDir, seedAttackForce, 20f, damageValue, "Player");
            
            yield return new WaitForSeconds(seedLaunchPeriod);
        }
    }

    /// <summary>
    /// Checks if any new plants can be spawned
    /// </summary>
    /// <returns>Whether a new plant can be spawned or not</returns>
    public bool CanSpawnNewPlant()
    {
        // Remove any dead plants from the list
        _spawnedPlants.RemoveAll(p => p.isDead);

        return _spawnedPlants.Count < maxPlantsSpawned;
    }
    
    /// <summary>
    /// Registers a newly spawned plant to be known by the boss
    /// </summary>
    /// <param name="plant">The newly spawned plant</param>
    public void AddPlantToSpawnList(GameObject plant)
    {
        EnemyStatsController plantStats = plant.GetComponent<EnemyStatsController>();
        
        if (plantStats)
            _spawnedPlants.Add(plantStats);
    }

    /// <summary>
    /// Kills all of the plants spawned by the Treant Boss
    /// </summary>
    public void KillAllSpawnedPlants()
    {
        foreach (EnemyStatsController plant in _spawnedPlants.Where(p => !p.isDead))
        {
            plant.Kill();
        }
        
        _spawnedPlants.Clear();
    }

    /// <summary>
    /// Will trigger the sequence for the Treant boss' special attack
    /// </summary>
    public override void SpecialAttack()
    {
        Anim.SetTrigger(Random.Range(0f, 1f) < 0.5f ? "SeedAttack" : "RootAttack");
        SpecialAttackTimer = specialAttackPeriod;
    }

    /// <summary>
    /// Treant boss attack choice strategy function
    /// </summary>
    /// <returns>Whether an attack was chosen or not</returns>
    public override bool ChooseAttack()
    {
        Transform target = Brain.GetCurrentTarget();
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < meleeDistance)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                AttackCooldown = primaryAttackCooldown;
                Anim.SetTrigger("BiteAttack");
            }
            else
            {
                AttackCooldown = secondaryAttackCooldown;
                Anim.SetTrigger("StompAttack");
            }
        }
        else if (distance < rangedDistance && Random.Range(0f, 1f) < 0.4f)
        {
            AttackCooldown = projectileAttackCooldown;
            Anim.SetTrigger("ProjectileAttack");
        }
        else
        {
            return false;
        }

        return true;
    }
}
