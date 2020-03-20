using System.Linq;
using UnityEngine;

public class PlantCombatController : EnemyCombatController
{
    [Header("Melee Attack")]
    public bool hasMeleeAttack = true;
    public float meleeAttackCooldown = 1f;
    public float meleeAttackRadius = 4f;
    public float meleeAttackAngle = 45f;
    [SerializeField] protected AudioHelper.EntityAudioClip meleeAttackSFX;

    /// <summary>
    /// Plant Melee Attack
    /// </summary>
    public override void PrimaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, meleeAttackSFX));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, meleeAttackRadius, meleeAttackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer()));
        }
    }

    /// <summary>
    /// Plant attack strategy.
    /// </summary>
    public override void ChooseAttack()
    {
        AttackCooldown = meleeAttackCooldown;
        Anim.SetTrigger("PrimaryAttack");
    }
}
