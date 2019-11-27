using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyBrainController))]
public class EnemyStatsController : EntityStatsController
{
    private EnemyBrainController _brain;
    private NavMeshAgent _agent;

    public GameObject FloatingText;

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

        if (characterColour != CharacterColour.None && attacker.characterColour != characterColour)
        {
            return;
        }

        // Calculate any changes based on stats and modifiers here first
        float hitValue = (damageValue - ComputeDefenseModifier()) * timeModifier;
        health.Subtract(hitValue < 0 ? 0 : hitValue);
        ShowDamage(hitValue);

        // Pass damage information to brain
        _brain.OnDamageTaken(attacker.gameObject, hitValue);
        
        Debug.Log(transform.name + " took " + hitValue + " damage.");

        if (Mathf.Approximately(health.CurrentValue, 0f))
        {
            Die();
        }
    }

    public void ShowDamage(float damage, float duration = 0.5f)
    {
        Vector3 offset = new Vector3(0, 5f, 0); // Want to do this dynamically based off enemy height
        float x = 1f, y = 0.5f;
        Vector3 random = new Vector3(Random.Range(-x, x), Random.Range(-y, y));

        GameObject text = Instantiate(FloatingText, transform.position + offset + random, Quaternion.identity, transform);
        text.GetComponent<TMP_Text>().text = damage.ToString("F1");

        Destroy(text, duration);
    }

    protected override IEnumerator ApplyExplosiveForce(float explosionForce, Vector3 explosionPoint, float explosionRadius, float stunTime)
    {
        // Set to stunned before applying explosive force
        SetStunned(true);
        rb.isKinematic = false;
        
        // TODO change this to AddForce(<force vector>, ForceMode.Impulse);
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
        _brain.SetStunned(isStunned);
    }
}
