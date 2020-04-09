using System.Linq;
using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    public delegate void OnActivation(bool isActivated);
    public OnActivation onActivation;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material defaultMaterial;
    protected Animator _anim;
    protected AudioSource _audio;
    
    public CharacterColour colour;
    protected bool _isActivated;
    
    protected void Awake()
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
            SetMaterial(colour);
        }
        else if (colour == CharacterColour.None)
        {
            colour = playerColours[Random.Range(0, playerColours.Length)];
            SetMaterial(colour);
        }
        else
        {
            // Turn off if object's colour isn't one of the players' colours
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter (Collider other) 
    {
        if (!_isActivated && other.CompareTag("Rock") && other.GetComponent<Interactable>().colour == colour)
        {
            _anim.SetTrigger("PlatformActivated");

            _audio.PlayDelayed(0);
            _isActivated = true;
            
            onActivation?.Invoke(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (_isActivated && other.CompareTag("Rock") && other.GetComponent<Interactable>().colour == colour)
        {
            _anim.enabled = true;
            _isActivated = false;
            
            onActivation?.Invoke(false);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Unhide()
    {
        gameObject.SetActive(true);
    }

    private void PausePlatformAnimationEvent()
    {
        _anim.enabled = false;
    }

    /// <summary>
    /// Set the material of the platform to reflect the assigned character colour
    /// </summary>
    private void SetMaterial(CharacterColour colour)
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
