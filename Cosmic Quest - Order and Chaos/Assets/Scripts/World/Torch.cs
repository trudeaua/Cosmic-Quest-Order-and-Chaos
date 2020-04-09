using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for player-torch interaction
public class Torch : Interactable
{
    // Delegate for when the torch is activated
    public delegate void OnInteract(bool isLit);
    public OnInteract onInteract;
    [SerializeField] private Material purpleMaterial;
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
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = new Material[1];
        Material material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        Texture texture;
        Color tintColor;
;       switch (colour) { 
            case CharacterColour.Purple:
                texture = purpleMaterial.GetTexture("_MainTex");
                tintColor = purpleMaterial.GetColor("_TintColor");
                break;
            case CharacterColour.Green:
                texture = greenMaterial.GetTexture("_MainTex");
                tintColor = greenMaterial.GetColor("_TintColor");
                break;
            case CharacterColour.Red:
                texture = redMaterial.GetTexture("_MainTex");
                tintColor = redMaterial.GetColor("_TintColor");
                break;
            case CharacterColour.Yellow:
                texture = yellowMaterial.GetTexture("_MainTex");
                tintColor = yellowMaterial.GetColor("_TintColor");
                break;
            default:
                texture = defaultMaterial.GetTexture("_MainTex");
                tintColor = defaultMaterial.GetColor("_TintColor");
                break;
        }
        material.SetTexture("_MainTex", texture);
        material.SetColor("_TintColor", tintColor);
        materials[0] = material;
        renderer.materials = materials;

        // Tint color turns to white if this isn't here
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

}