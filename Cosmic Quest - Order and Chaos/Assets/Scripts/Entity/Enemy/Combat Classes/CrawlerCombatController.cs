using System.Linq;
using UnityEngine;

public class CrawlerCombatController : EnemyCombatController
{
    [Header("Primary Attack - Lunge Attack")]
    public float primaryAttackCooldown = 1f;
    public float primaryAttackRadius = 4f;
    public float primaryAttackAngle = 45f;
    public float primaryAttackMinDamage = 1f;
    public float primaryAttackMaxDamage = 4f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    /// <summary>
    /// Crawler lunge attack.
    /// </summary>
    public override void PrimaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, primaryAttackSFX));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, primaryAttackRadius, primaryAttackAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(primaryAttackMinDamage, primaryAttackMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
    }

    /// <summary>
    /// Crawler attack strategy.
    /// </summary>
    public override void ChooseAttack()
    {
        AttackCooldown = primaryAttackCooldown;
        Anim.SetTrigger("PrimaryAttack");
    }
}