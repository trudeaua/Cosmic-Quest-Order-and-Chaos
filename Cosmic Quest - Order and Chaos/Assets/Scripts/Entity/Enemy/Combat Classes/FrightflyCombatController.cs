using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrightflyCombatController : EnemyCombatController
{
    [Header("Primary Attack - Bite Attack")]
    public float primaryAttackCooldown = 1f;
    public float primaryAttackRadius = 4f;
    public float primaryAttackAngle = 45f;
    public float primaryAttackMinDamage = 1f;
    public float primaryAttackMaxDamage = 4f;
    [Range(0f, 1f)] public float primaryAttackProbability = 0.7f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Secondary Attack - Stab Attack")]
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackRadius = 5f;
    public float secondaryAttackAngle = 45f;
    public float secondaryAttackMinDamage = 2f;
    public float secondaryAttackMaxDamage = 6f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;


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

    public override void SecondaryAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, secondaryAttackSFX));

        // Attack any enemies within the attack range (AOE-type effect)
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, secondaryAttackRadius, secondaryAttackAngle)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(secondaryAttackMinDamage, secondaryAttackMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
    }

    public override void ChooseAttack()
    {
        float randNum = Random.Range(0f, 1f);
        if (randNum <= primaryAttackProbability)
        {
            AttackCooldown = primaryAttackCooldown;
            Anim.SetTrigger("PrimaryAttack");
        }
        else
        {
            AttackCooldown = secondaryAttackCooldown;
            Anim.SetTrigger("SecondaryAttack");
        }
    }
}
