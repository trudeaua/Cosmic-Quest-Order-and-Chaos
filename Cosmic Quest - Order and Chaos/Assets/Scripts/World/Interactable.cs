using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base class for player interactable objects
/// </summary>
public class Interactable : MonoBehaviour
{
    [Tooltip("Whether the player must hold down the interact button to interact with this object")]
    public bool isHeld = false;

    [Tooltip("Whether the interaction is meant to be a triggered-style interaction")]
    public bool isTrigger = false;
    
    [Tooltip("Required character colour to interact with")]
    public CharacterColour colour = CharacterColour.All;

    [HideInInspector] public bool isParticleSystem;

    protected virtual void Start()
    {
        CharacterColour[] playerColours = PlayerManager.Instance.CurrentPlayerColours;
        if (playerColours.Contains(colour))
        {
            // Set the material colour of the interactable if not a particle system
            if (!isParticleSystem)
                SetMaterialColour(colour);
        }
        else if (colour == CharacterColour.None)
        {
            colour = playerColours[Random.Range(0, playerColours.Length)];
            SetMaterialColour(colour);
        }
        else
        {
            // Turn off if object's colour isn't one of the players' colours
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles the start of an interaction event with a player
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public virtual void StartInteract(Transform target)
    {
        // This function is intended to be overriden
        if (CanInteract(target))
        {
            Debug.Log("Started interaction with " + target.name);
        }
    }
    
    /// <summary>
    /// Handles the end of the interaction event with a player. This function is not necessary for one-time interactions.
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public virtual void StopInteract(Transform target)
    {
        // This function is intended to be overriden
        Debug.Log("Stopped interaction with " + target.name);
    }

    /// <summary>
    /// Determines if a given player is able to interact with this object
    /// </summary>
    /// <param name="target">The Transform attempting to interact with this object</param>
    /// <returns>Whether the Transform can interact with this object</returns>
    public virtual bool CanInteract(Transform target)
    {
        return colour == CharacterColour.All || target.GetComponent<EntityStatsController>().characterColour == colour;
    }

    public virtual void SetMaterialColour(CharacterColour characterColour)
    {
        Color color = PlayerManager.colours.GetColour(characterColour);
        Transform[] interactableComponents = GetComponentsInChildren<Transform>();
        float intensity = 0.1f;
        Material[] materials = new Material[1];
        materials[0] = new Material(Shader.Find("Standard"));
        materials[0].EnableKeyword("_EMISSION");
        materials[0].SetColor("_Color", color);
        materials[0].SetColor("_EmissionColor", color * intensity);
        foreach (Transform interactableComponent in interactableComponents)
        {
            Renderer[] interactableRenderers = interactableComponent.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in interactableRenderers)
            {
                r.materials = materials;
            }
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = PlayerManager.colours.GetColour(colour);
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach(MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            mesh.RecalculateNormals();
            Gizmos.DrawWireMesh(mesh, meshFilter.transform.position, meshFilter.transform.rotation, meshFilter.transform.lossyScale);
        }
    }
}
