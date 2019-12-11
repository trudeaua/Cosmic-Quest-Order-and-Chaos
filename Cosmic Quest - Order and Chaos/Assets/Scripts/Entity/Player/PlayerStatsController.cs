using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public RegenerableStat mana;
    
    // player collider
    private Collider thisCollider;
    
    // ragdoll collider
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    protected override void Awake()
    {
        base.Awake();
        mana.Init();
        
        // get the collider attached to the player
        thisCollider = GetComponent<Collider>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(false);
        Anim.enabled = true;
    }

    private void Start()
    {
        // colour the player's weapon
        AssignWeaponColour(gameObject, PlayerManager.colours.GetColour(characterColour));
    }

    protected override void Update()
    {
        base.Update();

        if (!isDead)
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

    private void AssignWeaponColour(GameObject player, Color color)
    {
        // Get the player weapon
        Transform[] children = player.GetComponentsInChildren<Transform>();
        GameObject weapon = null;
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].CompareTag("Weapon"))
            {
                weapon = children[i].gameObject;
                break;
            }
        }
        // Dynamically assign player weapon colours
        if (weapon != null)
        {
            Transform[] weaponComponents = weapon.GetComponentsInChildren<Transform>();
            float intensity = 2.0f;
            foreach (Transform weaponComponent in weaponComponents)
            {
                if (weaponComponent.CompareTag("Weapon Glow"))
                {
                    Material[] weaponMaterials = weaponComponent.GetComponent<Renderer>().materials;
                    // the bow has more than 1 material assigned to one of its weapon parts
                    foreach (Material m in weaponMaterials)
                    {
                        m.EnableKeyword("_EMISSION");
                        m.SetColor("_Color", color);
                        m.SetColor("_EmissionColor", color * intensity);
                    }
                }
            }
        }
    }
}
