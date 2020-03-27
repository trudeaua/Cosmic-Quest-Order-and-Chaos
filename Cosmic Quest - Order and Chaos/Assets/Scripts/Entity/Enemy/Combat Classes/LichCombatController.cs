using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LichCombatController : EnemyCombatController
{
    [Header("Primary Attack - Lunge Attack")]
    public float primaryAttackCooldown = 1f;
    public float primaryAttackRadius = 4f;
    public float primaryAttackAngle = 45f;
    public float primaryAttackMinDamage = 3f;
    public float primaryAttackMaxDamage = 8f;
    [Range(0f, 1f)] public float primaryAttackProbability = 0.7f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Secondary Attack - Spell Cast")]
    public float secondaryAttackCooldown = 2f;
    public float secondaryAttackRadius = 5f;
    public float secondaryAttackMinDamage = 3f;
    public float secondaryAttackMaxDamage = 7f;
    public GameObject secondaryAttackVFX;
    public Transform secondaryAttackVFXRoot;
    [SerializeField] protected AudioHelper.EntityAudioClip secondaryAttackSFX;
    private bool _isSecondaryActive;

    private RaycastHit[] _hitBuffer = new RaycastHit[32];
    
    protected override void Update()
    {
        base.Update();

        if (_isSecondaryActive)
        {
            SecondaryAttack();
        }
    }

    /// <summary>
    /// Lich's lunge attack
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
    /// Event function to start the Lich's spell cast
    /// </summary>
    public void StartSecondaryAttack()
    {
        _isSecondaryActive = true;
    }

    /// <summary>
    /// Event function to stop the Lich's spell cast
    /// </summary>
    public void StopSecondaryAttack()
    {
        _isSecondaryActive = false;
        AttackCooldown = secondaryAttackCooldown;
    }
    
    /// <summary>
    /// Lich's spell cast attack
    /// </summary>
    public override void SecondaryAttack()
    {
        // Calculate direction to attack in relative to staff position
        Vector3 direction = secondaryAttackVFXRoot.up;
        direction.y = 0f;
        Vector3 position = secondaryAttackVFXRoot.position;
        position.y = 1f;

        // Spawn VFX from staff and in the direction its pointing
        Vector3 vfxPos = position + direction * 0.5f + new Vector3(0, 2f);
        StartCoroutine(VfxHelper.CreateVFX(secondaryAttackVFX, vfxPos, Quaternion.LookRotation(direction)));

        // Calculate the hit raycast from closer to the enemy, in the direction of the staff
        Vector3 raycastPos = position - direction * 2.5f;
        int numHits = Physics.RaycastNonAlloc(raycastPos, direction, _hitBuffer, secondaryAttackRadius);
        float damageValue = Random.Range(secondaryAttackMinDamage, secondaryAttackMaxDamage) + Stats.damage.GetValue();

        // Damage any players hit by the raycast
        for (int i = 0; i < numHits; i++)
        {
            if (_hitBuffer[i].transform.tag.Equals("Player"))
                _hitBuffer[i].transform.GetComponent<EntityStatsController>().TakeDamage(Stats, damageValue, Time.deltaTime);
        }
    }

    public override void ChooseAttack()
    {
        // Use primary attack, unless surrounded by more than one player
        if (Players.Count(player => CanDamageTarget(player, secondaryAttackRadius, 180f)) > 1)
        {
            float randNum = Random.Range(0f, 1f);

            if (randNum < primaryAttackProbability)
            {
                Anim.SetTrigger("PrimaryAttack");
                AttackCooldown = primaryAttackCooldown;
            }
            else
            {
                Anim.SetTrigger("SecondaryAttack");
                AttackCooldown = secondaryAttackCooldown;
            }
        }
        else
        {
            Anim.SetTrigger("PrimaryAttack");
            AttackCooldown = primaryAttackCooldown;
        }
    }
}
