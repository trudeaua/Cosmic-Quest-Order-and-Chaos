using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for player-torch interaction
public class Torch : Interactable
{
    // Delegate for when the torch is activated
    public delegate void OnInteract(bool isLit);
    public OnInteract onInteract;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material defaultMaterial;
    public bool isLit;
    public GameObject flameVFX;
    
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        flameVFX.SetActive(isLit);
    }

    /// <summary>
    /// Enforce interactable defaults
    /// </summary>
    private void Reset()
    {
        // Set default value for interactable fields
        isTrigger = true;
        isHeld = false;
    }
    
    /// <summary>
    /// Interact with the torch, flipping its current state
    /// </summary>
    /// <param name="target">Target that is trying to interact with the torch</param>
    public override void StartInteract(Transform target)
    {
        if (CanInteract(target))
        {
            _audio.PlayDelayed(0);

            SetLit(!isLit);
        }
    }

    /// <summary>
    /// API for controlling the torch state from a script
    /// </summary>
    /// <param name="lit">Whether to set the torch to be lit or not</param>
    public void SetLit(bool lit)
    {
        // Exit if state is already set
        if (isLit == lit)
            return;
        
        isLit = lit;
        onInteract?.Invoke(isLit);
        flameVFX.SetActive(isLit);
    }

     public override void SetMaterialColour(CharacterColour colour)
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        Material material;
        switch (colour) { 
            case CharacterColour.Blue:
             		material = blueMaterial;
                break;
            case CharacterColour.Green:
            		material = greenMaterial;
                break;
            case CharacterColour.Red:
            		material = redMaterial;
                break;
            case CharacterColour.Yellow:
            		material = yellowMaterial;
                break;
            default:
            		material = defaultMaterial;
                break;
        }
        
        renderer.material = material;

    }

}