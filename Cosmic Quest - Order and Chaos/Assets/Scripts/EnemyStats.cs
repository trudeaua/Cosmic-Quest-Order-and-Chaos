using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    public override void TakeDamage(EntityStats attacker, int damage)
    {
        // TODO keep track of who did damage to the enemy last?

        if (characterColour != CharacterColour.None && attacker.characterColour == characterColour)
        {
            Debug.Log("Attack was ineffective against this colour!");
            return;
        }

        // Calculate any changes based on stats and modifiers here first
        currentHealth -= damage;
        Debug.Log(transform.name + " took " + damage + " damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log(transform.name + " died.");
        isDead = true;
        StartCoroutine(EnemyDeath());
    }

    // TODO need to disable enemy on death and just show animation
    IEnumerator EnemyDeath()
    {
        GetComponentInChildren<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(1.1f);
        transform.gameObject.SetActive(false);
    }
}
