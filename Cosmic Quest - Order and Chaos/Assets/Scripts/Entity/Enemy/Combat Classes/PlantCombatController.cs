using System.Linq;
using UnityEngine;

public class PlantCombatController : EnemyCombatController
{
    [Header("Melee Attack")]
    public bool hasMeleeAttack = true;
    public float meleeAttackCooldown = 1f;
    public float meleeAttackRadius = 4f;
    public float meleeAttackAngle = 45f;
    public float meleeAttackMinDamage = 1f;
    public float meleeAttackMaxDamage = 4f;
    [SerializeField] protected AudioHelper.EntityAudioClip meleeAttackSFX;

    [Header("Ranged Attack")]
    public bool hasRangedAttack = false;
    public float rangedAttackCooldown = 1f;
    public float rangedAttackMinDamage = 1f;
    public float rangedAttackMaxDamage = 4f;
    public GameObject rangedAttackProjectile;
    [SerializeField] protected AudioHelper.EntityAudioClip rangedAttackSFX;
    
    /// <summary>
    /// Plant Melee Attack
    /// </summary>
    public override void PrimaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, meleeAttackSFX));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, meleeAttackRadius, meleeAttackAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(meleeAttackMinDamage, meleeAttackMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
    }

    /// <summary>
    /// Plant ranged attack
    /// </summary>
    public override void SecondaryAttack()
    {
        Transform target = Brain.GetCurrentTarget();

        if (target is null)
            return;
        
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, rangedAttackSFX));
        
        // Launch projectile to target player
        float damageValue = Random.Range(rangedAttackMinDamage, rangedAttackMaxDamage) + Stats.damage.GetValue();
        LaunchDamageProjectile(rangedAttackProjectile, target.position - transform.position, 500f, 20f, damageValue, "Player");
    }

    /// <summary>
    /// Plant attack strategy
    /// </summary>
    /// <returns>Whether an attack was chosen or not</returns>
    public override bool ChooseAttack()
    {
        float targetDistance = Vector3.Distance(transform.position, Brain.GetCurrentTarget().position);
        
        if (hasMeleeAttack && targetDistance <= Brain.attackRadius)
        {
            // Perform melee attack if player is close enough
            AttackCooldown = meleeAttackCooldown;
            Anim.SetTrigger("PrimaryAttack");
        }
        else if (hasRangedAttack && targetDistance <= Brain.aggroRadius)
        {
            // Perform ranged attack if player is within aggro radius
            AttackCooldown = rangedAttackCooldown;
            Anim.SetTrigger("SecondaryAttack");
        }

        return true;
    }
}
