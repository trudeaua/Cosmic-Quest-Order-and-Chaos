using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public RegenerableStat mana;

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

        if (shouldSpawn)
        {
            // Create a VFX where the player will spawn - just slightly above the stage (0.1f) - and change the VFX colour to match the player colour
            StartCoroutine(VfxHelper.CreateVFX(spawnVFX, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity, playerColour, 0.5f));
            // "Spawn" the player (they float up through the stage)
            StartCoroutine(Spawn(gameObject, spawnSpeed, spawnDelay, spawnCooldown));
        }
    }

    protected override void Update()
    {
        base.Update();

        if (!isDead) {
            health.Regen();
            mana.Regen();
        }
    }

    /// <summary>
    /// Kill the player
    /// </summary>
    protected override void Die()
    {
        Debug.Log(transform.name + " died.");
        isDead = true;
        Anim.enabled = false;
        EnableRagdoll(true);
        StartCoroutine(PlayerDeath());
        StartCoroutine(AudioHelper.PlayAudioOverlap(VocalAudio, entityDeathVocalSFX));
    }

    /// <summary>
    /// Make the player inactive
    /// </summary>
    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(5.5f);
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggle the player ragdoll
    /// </summary>
    /// <param name="enable">Indicates whether the ragdoll should be enabled or not</param>
    private void EnableRagdoll(bool enable)
    {
        // toggle the rigidbodies on the character limbs
        foreach (Rigidbody rrb in ragdollRigidbodies)
        {
            rrb.isKinematic = !enable;
        }
        // toggle the kinematic state of the player to be the opposite of the kinematic state in the character limbs
        rb.isKinematic = enable;

        // similar stuff for colliders
        foreach (Collider rcol in ragdollColliders)
        {
            rcol.enabled = enable;
        }
        _collider.enabled = !enable;
    }

    /// <summary>
    /// Assign the player's weapon colour
    /// </summary>
    /// <param name="player">Player gameobject</param>
    /// <param name="colour">Colour to set the player's weapon to</param>
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
    
    /// <summary>
    /// "Spawn" the player by causing them to float up through the stage
    /// </summary>
    /// <param name="obj">Object to spawn</param>
    /// <param name="speed">How fast the spawn should be</param>
    /// <param name="delay">How many seconds to wait before spawning</param>
    /// <param name="cooldown">How many seconds to wait before enabling the enemy's movement</param>
    protected override IEnumerator Spawn(GameObject obj, float speed = 0.05f, float delay = 0f, float cooldown = 0)
    {
        // disable player input until spawn sequence is done
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.PassivateInput();
        yield return base.Spawn(obj, speed, delay, cooldown);
        playerInput.ActivateInput();
    }
}