using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBossCombatController : EnemyCombatController
{
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackDelay = 0.6f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;

    public float tertiaryAttackCooldown = 1f;
    public float tertiaryAttackDelay = 0.6f;
    public GameObject tertiaryAttackVFX;
    [SerializeField] protected AudioHelper.EntityAudioClip tertiaryAttackSFX;

    /// <summary>
    /// Enemy boss's primary attack
    /// </summary>
    public override void PrimaryAttack()
    {
        if (AttackCooldown > 0f)
            return;

        Anim.SetTrigger("PrimaryAttack");
        // audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, primaryAttackSFX));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), primaryAttackDelay));
        }
        AttackCooldown = primaryAttackCooldown;
    }

    /// <summary>
    /// Enemy boss's secondary attack
    /// </summary>
    public override void SecondaryAttack()
    {
        if (AttackCooldown > 0f)
            return;

        Anim.SetTrigger("SecondaryAttack");
        // audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, secondaryAttackSFX));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), secondaryAttackDelay));
        }
        AttackCooldown = secondaryAttackCooldown;
    }
    /// <summary>
    /// Enemy boss's tertiary attack
    /// </summary>
    public override void TertiaryAttack()
    {
        if (AttackCooldown > 0f)
            return;

        Anim.SetTrigger("TertiaryAttack");
        // audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, tertiaryAttackSFX));

        StartCoroutine(VfxHelper.CreateVFX(tertiaryAttackVFX, gameObject.transform.position + new Vector3(0, 1f, 0), gameObject.transform.rotation,
            PlayerManager.colours.GetColour(Stats.characterColour), tertiaryAttackDelay * 0.5f));

        // Attack any enemies within the attack sweep and range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player.transform.position, attackRadius, attackAngle)))
        {
            // Calculate and perform damage
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), tertiaryAttackDelay * 0.5f));
        }
        AttackCooldown = tertiaryAttackCooldown;
    }
}