using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public RegenerableStat mana;
    
    // Player spawn VFX
    public GameObject spawnVFX;
    
    // player collider
    private Collider _collider;
    
    // ragdoll collider
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    protected override void Awake()
    {
        base.Awake();
        mana.Init();
        
        // get the collider attached to the player
        _collider = GetComponent<Collider>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(false);
        Anim.enabled = true;
    }

    private void Start()
    {
        Color playerColour = PlayerManager.colours.GetColour(characterColour);
        
        // colour the player's weapon
        AssignWeaponColour(gameObject, playerColour);
        
        // Create a VFX where the player will spawn - just slightly above the stage (0.1f) - and change the VFX colour to match the player colour
        StartCoroutine(VfxHelper.CreateVFX(spawnVFX, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.identity, playerColour, 0.5f));
        // "Spawn" the player (they float up through the stage)
        StartCoroutine(Spawn(gameObject, 0.08f, 0.9f));
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
        foreach (Rigidbody rrb in ragdollRigidbodies)
        {
            rrb.isKinematic = !enable;
        }
        rb.isKinematic = enable;

        foreach (Collider rcol in ragdollColliders)
        {
            rcol.enabled = enable;
        }
        _collider.enabled = !enable;
    }

    private void AssignWeaponColour(GameObject player, Color color)
    {
        // Get the player weapon
        Transform[] children = player.GetComponentsInChildren<Transform>();
        GameObject weapon = null;
        
        foreach (var child in children)
        {
            if (child.CompareTag("Weapon"))
            {
                weapon = child.gameObject;
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
