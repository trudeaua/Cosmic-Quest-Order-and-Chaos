using System.Linq;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    public delegate void OnActivation(bool isActivated);
    public OnActivation onActivation;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material defaultMaterial;
    private Animator _anim;
    private AudioSource _audio;
    
    public CharacterColour colour;
    private bool _isActivated;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        CharacterColour[] playerColours = PlayerManager.Instance.CurrentPlayerColours;
        if (playerColours.Contains(colour))
        {
            // Set the material colour of the platform
            SetMaterial();
        }
        else
        {
            // Turn off if object's colour isn't one of the players' colours
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter (Collider other) 
    {
        if (!_isActivated && other.CompareTag("Rock") && other.GetComponent<Interactable>().colour == colour)
        {
            _anim.SetTrigger("PlatformActivated");

            _audio.PlayDelayed(0);
            _isActivated = true;
            
            onActivation?.Invoke(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_isActivated && other.CompareTag("Rock") && other.GetComponent<Interactable>().colour == colour)
        {
            _anim.enabled = true;
            _isActivated = false;
            
            onActivation?.Invoke(false);
        }
    }

    private void PausePlatformAnimationEvent()
    {
        _anim.enabled = false;
    }

    /// <summary>
    /// Set the material of the platform to reflect the assigned character colour
    /// </summary>
    private void SetMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = new Material[1];
        Material material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        Texture texture;
        Color tintColor;
;       switch (colour) { 
            case CharacterColour.Blue:
                texture = blueMaterial.GetTexture("_MainTex");
                tintColor = blueMaterial.GetColor("_TintColor");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = PlayerManager.colours.GetColour(colour);
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            mesh.RecalculateNormals();
            Gizmos.DrawWireMesh(mesh, meshFilter.transform.position, meshFilter.transform.rotation, meshFilter.transform.lossyScale);
        }
    }
}
