using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public RegenerableStat stamina;
    public RegenerableStat mana;
    // player collider
    private Collider thisCollider;
    // ragdoll collider
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    protected override void Awake()
    {
        base.Awake();
        // get the collider attached to the player
        thisCollider = GetComponent<Collider>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(false);
        Anim.enabled = true;
    }


    protected override void Update()
    {
        base.Update();
        
        stamina.Regen();
        mana.Regen();
    }
    
    protected override void Die()
    {
        Debug.Log(transform.name + " died.");
        isDead = true;
        Anim.enabled = false;
        EnableRagdoll(true);
        StartCoroutine(PlayerDeath());
    }
    
    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(5.5f);
        transform.gameObject.SetActive(false);
    }

    private void EnableRagdoll(bool enable)
    {
        foreach (Rigidbody rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = !enable;
        }
        rb.isKinematic = enable;

        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = enable;
        }
        thisCollider.enabled = !enable;
    }
}
