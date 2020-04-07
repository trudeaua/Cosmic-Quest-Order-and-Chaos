using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    public delegate void OnActivation(bool isActivated);
    public OnActivation onActivation;
    
    private Animator _anim;
    private AudioSource _audio;
    
    public CharacterColour colour;
    private bool _isActivated;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
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

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Unhide()
    {
        gameObject.SetActive(true);
    }
}
