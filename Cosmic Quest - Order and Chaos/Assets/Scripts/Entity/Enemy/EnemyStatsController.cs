using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsController : EntityStatsController
{
    public override void TakeDamage(EntityStatsController attacker, float damageValue)
    {
        // ignore attacks if already dead
        if (isDead)
            return;

        // TODO keep track of who did damage to the enemy last?

        if (characterColour != CharacterColour.None && attacker.characterColour == characterColour)
        {
            Debug.Log("Attack was ineffective against this colour!");
            return;
        }

        // Calculate any changes based on stats and modifiers here first
        float hitValue = damageValue - ComputeDefenseModifier();
        health.Subtract(hitValue < 0 ? 0 : hitValue);
        Debug.Log(transform.name + " took " + hitValue + " damage.");

        if (Mathf.Approximately(health.currentValue, 0f))
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log(transform.name + " died.");
        isDead = true;
        StartCoroutine(EnemyDeath());
    }

    // TODO need to disable enemy on death and just show animation
    private IEnumerator EnemyDeath()
    {
        GetComponentInChildren<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(1.1f);
        transform.gameObject.SetActive(false);
    }
}
