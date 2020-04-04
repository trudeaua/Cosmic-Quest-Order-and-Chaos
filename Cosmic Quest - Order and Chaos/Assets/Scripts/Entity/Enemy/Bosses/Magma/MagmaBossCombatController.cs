using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MagmaBossCombatController : EnemyCombatController
{
    [Header("Punch Attack")]
    public float primaryAttackCooldown = 1f;
    public float primaryAttackRadius = 4f;
    public float primaryAttackAngle = 45f;
    public float primaryAttackMinDamage = 2f;
    public float primaryAttackMaxDamage = 6f;
    [Range(0f, 1f)] public float primaryAttackProbability = 0.7f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Swing Attack")]
    public float secondaryAttackCooldown = 1f;
    public float secondaryAttackRadius = 5f;
    public float secondaryAttackAngle = 200f;
    public float secondaryAttackMinDamage = 5f;
    public float secondaryAttackMaxDamage = 10f;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;
    
    [Header("Explosion Attack")]
    public float explosionAttackCooldown = 1f;
    public float explosionAttackRadius = 5f;
    public float explosionAttackMinDamage = 5f;
    public float explosionAttackMaxDamage = 10f;
    [Tooltip("Probability of performing the explosion attack if 2 or more players are nearby")]
    [Range(0f, 1f)] public float explosionAttackProbability = 0.3f;
    [SerializeField] protected AudioHelper.EntityAudioClip explosionAttackSFX;

    [Header("Special Attack - Fire Storm")]
    [Tooltip("How often the boss will perform its special attack")]
    public float specialAttackPeriod = 20f;
    public float firestormCooldown = 1f;
    public float firestormRadius = 5f;
    public float firestormMinDamage = 10f;
    public float firestormMaxDamage = 30f;
    [SerializeField] protected AudioHelper.EntityAudioClip firestormSFX;
    public Torch[] arenaTorches;

    private float _specialAttackTimer;

    public bool CanUseSpecialAttack
    {
        get { return _specialAttackTimer <= 0f; }
    }

    private void Start()
    {
        _specialAttackTimer = specialAttackPeriod;
    }

    protected override void Update()
    {
        base.Update();

        if (_specialAttackTimer > 0)
            _specialAttackTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Punch attack.
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
    /// Swing attack.
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
    /// Attack event for the special charge attack
    /// </summary>
    public void ExplosionAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, explosionAttackSFX));

        // Attack any players within the attack range
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, explosionAttackRadius)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(explosionAttackMinDamage, explosionAttackMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
    }

    /// <summary>
    /// Damage event for the boss' special firestorm attack.
    /// This attack launches an AOE firestorm effect, as well as
    /// lights some of the torches surrounding the boss arena, causing
    /// fireballs to fall down from the sky.
    /// </summary>
    public void FirestormAttack()
    {
        // Play attack audio
        StartCoroutine(AudioHelper.PlayAudioOverlap(WeaponAudio, firestormSFX));

        // Damage any players within the AOE
        foreach (GameObject player in Players.Where(player => CanDamageTarget(player, firestormRadius)))
        {
            // Calculate and perform damage
            float damageValue = Random.Range(firestormMinDamage, firestormMaxDamage) + Stats.damage.GetValue();
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), damageValue));
        }
        
        // Light a random amount of torches surrounding the boss arena
        foreach (Torch torch in arenaTorches)
        {
            if (Random.Range(0f, 1f) > 0.3f)
                torch.SetLit(true);
        }

        // Set the cooldown for a followup attack
        AttackCooldown = firestormCooldown;
    }

    /// <summary>
    /// Magma boss attack choice strategy function
    /// </summary>
    public override void ChooseAttack()
    {
        if (Players.Count(player => CanDamageTarget(player, explosionAttackRadius)) > 1 && Random.Range(0f, 1f) <= explosionAttackProbability)
        {
            // If atleast 2 players can be affected by the explosion attack, use a different random number roll
            // to determine if the boss will perform this attack.
            AttackCooldown = explosionAttackCooldown;
            Anim.SetTrigger("ExplosionAttack");
        }
        else if (Random.Range(0f, 1f) <= primaryAttackProbability)
        {
            AttackCooldown = primaryAttackCooldown;
            Anim.SetTrigger("PunchAttack");
        }
        else
        {
            AttackCooldown = secondaryAttackCooldown;
            Anim.SetTrigger("SwingAttack");
        }
    }
}