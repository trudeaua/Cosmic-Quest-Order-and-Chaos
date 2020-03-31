﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerpentBossCombatController : EnemyCombatController
{
    [Header("Claw Attack")]
    public float primaryAttackCooldown = 1f;
    public float primaryAttackRadius = 4f;
    public float primaryAttackAngle = 45f;
    public float primaryAttackMinDamage = 2f;
    public float primaryAttackMaxDamage = 6f;
    [Range(0f, 1f)] public float primaryAttackProbability = 0.7f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Bite Attack")]
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackRadius = 5f;
    public float secondaryAttackAngle = 200f;
    public float secondaryAttackMinDamage = 5f;
    public float secondaryAttackMaxDamage = 10f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;

    /// <summary>
    /// Claw attack.
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
    /// Bite attack.
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
    /// Serpent boss attack choice strategy function
    /// </summary>
    public override void ChooseAttack()
    {
        float randNum = Random.Range(0f, 1f);
        if (randNum <= primaryAttackProbability)
        {
            AttackCooldown = primaryAttackCooldown;
            Anim.SetTrigger(Random.Range(0f, 1f) < 0.5f ? "ClawLeftAttack" : "ClawRightAttack");
        }
        else
        {
            AttackCooldown = secondaryAttackCooldown;
            Anim.SetTrigger("BiteAttack");
        }
    }
}
