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
    [Range(0f, 1f)] public float primaryAttackProbability = 0.7f;
    [SerializeField] protected AudioHelper.EntityAudioClip primaryAttackSFX;
    
    [Header("Secondary Attack - Spell Cast")]
    public float secondaryAttackCooldown = 2f;
    public float secondaryAttackRadius = 5f;
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
            StartCoroutine(PerformDamage(player.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer()));
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
        Vector3 direction = secondaryAttackVFXRoot.rotation * transform.forward;
        Vector3 position = secondaryAttackVFXRoot.position;
        position.y = 1f;
        
        Debug.DrawRay(position, secondaryAttackRadius * direction, Color.red);

        /*Vector3 vfxPos = transform.position + transform.forward * 1.6f + new Vector3(0, 2f);
        StartCoroutine(VfxHelper.CreateVFX(secondaryAttackVFX, vfxPos, transform.rotation));

        int numHits = Physics.RaycastNonAlloc(position, direction, _hitBuffer, secondaryAttackRadius);

        // Damage any players hit by the raycast
        for (int i = 0; i < numHits; i++)
        {
            if (_hitBuffer[i].transform.tag.Equals("Player"))
                _hitBuffer[i].transform.GetComponent<EntityStatsController>().TakeDamage(Stats, Stats.ComputeDamageModifer(), Time.deltaTime);
        }*/
    }

    public override void ChooseAttack()
    {
        //Anim.SetTrigger("PrimaryAttack");
        Anim.SetTrigger("SecondaryAttack");
        AttackCooldown = primaryAttackCooldown;
    }
}
