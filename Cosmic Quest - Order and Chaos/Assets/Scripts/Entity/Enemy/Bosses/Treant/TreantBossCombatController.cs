using System.Collections;
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
    public float projectileAttackForce = 200f;
    public float projectileAttackAngle = 120f;
    public int projectileAttackNumSeeds = 3;
    public float projectileAttackMinDamage = 5f;
    public float projectileAttackMaxDamage = 10f;
    public GameObject seedPrefab;
    [SerializeField] protected AudioHelper.EntityAudioClip projectileAttackSFX;
    
    [Header("Special Attack - Roots")]
    public float rootAttackCooldown = 1f;
    public GameObject rootContainerPrefab;
    [SerializeField] protected AudioHelper.EntityAudioClip rootAttackSFX;
    private TreantRootContainer _roots;
    
    [Header("Special Attack - Launch Seeds")]
    public float seedAttackCooldown = 1f;
    public float seedAttackForce = 200f;
    public float seedAttackAngle = 160f;
    public float seedAttackMinDamage = 5f;
    public float seedAttackMaxDamage = 10f;
    public float seedLaunchPeriod = 0.2f;
    [SerializeField] protected AudioHelper.EntityAudioClip seedAttackSFX;
    private bool _isLaunchingSeeds;

    protected override void Awake()
    {
        base.Awake();

        GameObject roots = Instantiate(rootContainerPrefab);
        _roots = roots.GetComponent<TreantRootContainer>();
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
    }

    private IEnumerator SeedLaunchAttack()
    {
        while (_isLaunchingSeeds)
        {
            // Select random direction to launch in
            float breadth = Random.Range(-seedAttackAngle, seedAttackAngle) / 2;
            float altitude = Random.Range(0f, 80f);
            Vector3 launchDir = Quaternion.AngleAxis(breadth, Vector3.up) * Quaternion.AngleAxis(altitude, transform.right) * transform.forward;
            
            // Launch the seed projectile
            float damageValue = Random.Range(seedAttackMinDamage, seedAttackMaxDamage) + Stats.damage.GetValue();
            LaunchDamageProjectile(seedPrefab, launchDir, projectileAttackForce, 20f, damageValue, "Player");
            
            yield return new WaitForSeconds(seedLaunchPeriod);
        }
    }

    /// <summary>
    /// Will trigger the sequence for the Treant boss' special attack
    /// </summary>
    public override void SpecialAttack()
    {
        Anim.SetTrigger("RootAttack");
        SpecialAttackTimer = specialAttackPeriod;
    }

    /// <summary>
    /// Treant boss attack choice strategy function
    /// </summary>
    public override void ChooseAttack()
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
}
