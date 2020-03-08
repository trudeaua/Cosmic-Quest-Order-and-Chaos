using System.Linq;
using UnityEngine;

public class MetalonCombatController : EnemyCombatController
{
    [Header("Primary Attack - Stab Attack")]
    public float primaryAttackCooldown = 1f;
    public float primaryAttackRadius = 4f;
    public float primaryAttackAngle = 45f;
    [Range(0f, 1f)] public float primaryAttackProbability = 0.7f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Secondary Attack - Slam Attack")]
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackRadius = 5f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;

    /// <summary>
    /// Metalon quick stab attack.
    /// </summary>
    public override void PrimaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, primaryAttackSFX));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, primaryAttackRadius, primaryAttackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer()));
        }
    }

    /// <summary>
    /// Metalon slam attack.
    /// </summary>
    public override void SecondaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, secondaryAttackSFX));

        // Attack any enemies within the attack range (AOE-type effect)
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, secondaryAttackRadius)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer()));
        }
    }

    /// <summary>
    /// Metalon attack choice strategy function. Selects an attack by a weighted random.
    /// </summary>
    public override void ChooseAttack()
    {
        float randNum = Random.Range(0f, 1f);
        if (randNum <= primaryAttackProbability)
        {
            Anim.SetTrigger("PrimaryAttack");
        }
        else
        {
            Anim.SetTrigger("SecondaryAttack");
        }
    }
}