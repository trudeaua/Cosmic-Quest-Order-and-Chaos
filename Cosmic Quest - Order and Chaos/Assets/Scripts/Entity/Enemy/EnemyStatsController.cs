using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyBrainController))]
public class EnemyStatsController : EntityStatsController
{
    private EnemyBrainController _brain;
    private NavMeshAgent _agent;

    protected override void Awake()
    {
        base.Awake();

        _brain = GetComponent<EnemyBrainController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void TakeDamage(EntityStatsController attacker, float damageValue, float timeModifier = 1f)
    {
        // Ignore attacks if already dead
        if (isDead)
            return;

        if (characterColour != CharacterColour.None && attacker.characterColour == characterColour)
        {
            Debug.Log("Attack was ineffective against this colour!");
            return;
        }

        // Calculate any changes based on stats and modifiers here first
        float hitValue = (damageValue - ComputeDefenseModifier()) * timeModifier;
        health.Subtract(hitValue < 0 ? 0 : hitValue);
        
        // Pass damage information to brain
        _brain.OnDamageTaken(attacker.gameObject, hitValue);
        
        Debug.Log(transform.name + " took " + hitValue + " damage.");

        if (Mathf.Approximately(health.CurrentValue, 0f))
        {
            Die();
        }
    }
    
    public override void TakeExplosionDamage(EntityStatsController attacker, float maxDamage, float stunTime, 
        float explosionForce, Vector3 explosionPoint, float explosionRadius)
    {
        // Ignore explosions if already dead
        if (isDead)
            return;
        
        // Calculate damage based on distance from the explosion point
        float relativeDamage = (Vector3.Distance(explosionPoint, transform.position) / explosionRadius) * maxDamage;
        TakeDamage(attacker, relativeDamage);

        StartCoroutine(ApplyExplosiveForce(explosionForce, explosionPoint, explosionRadius, stunTime));
    }

    private IEnumerator ApplyExplosiveForce(float explosionForce, Vector3 explosionPoint, float explosionRadius,
        float stunTime)
    {
        // Set to stunned before applying explosive force
        SetStunned(true);
        rb.isKinematic = false;
        rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
        
        // Wait for a moment before re-enabling the navMeshAgent
        yield return new WaitForSeconds(stunTime);
        rb.isKinematic = true;
        SetStunned(false);
    }

    protected override void Die()
    {
        Debug.Log(transform.name + " died.");
        isDead = true;
        _agent.enabled = false;
        StartCoroutine(EnemyDeath());
    }

    private IEnumerator EnemyDeath()
    {
        Anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.1f);
        transform.gameObject.SetActive(false);
    }

    private void SetStunned(bool isStunned)
    {
        // Disable the nav and stun the brain
        _agent.enabled = !isStunned;
        _agent.updatePosition = !isStunned;
        _agent.updateRotation = !isStunned;
        _brain.SetStunned(isStunned);
    }
}
