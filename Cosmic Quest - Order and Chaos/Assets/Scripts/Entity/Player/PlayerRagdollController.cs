using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollController : MonoBehaviour
{
    private Animator Anim;
    private Collider thisCollider;
    private Collider[] ragdollColliders;
    private PlayerStatsController playerStats;

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStatsController>();
        // get the collider attached to the player
        thisCollider = GetComponent<Collider>();
        // get all ragdoll colliders
        ragdollColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in ragdollColliders)
        {
            // keep any weapon colliders
            if (col.gameObject.tag != "Weapon")
            {
                col.enabled = false;
            }
            else
            {
                col.enabled = true;
            }
        }
        thisCollider.enabled = true;
    }

    protected void Update()
    {
        if (playerStats.isDead)
        {
            StartCoroutine(EnableRagdoll());
        }
    }

    private IEnumerator EnableRagdoll()
    {
        // Anim.SetTrigger("Die");
        thisCollider.enabled = false;
        Anim.enabled = false;
        // enable the ragdoll colliders
        foreach (Collider col in ragdollColliders)
        {
            if (col.gameObject.tag != "Weapon")
            {
                // enable all ragdoll colliders
                col.enabled = true;
            }
            else
            {
                // don't let a dead player's weapon hit an enemy
                col.enabled = false;
            }
        }
        yield return new WaitForSeconds(5.5f);
        transform.gameObject.SetActive(false);
    }
}
