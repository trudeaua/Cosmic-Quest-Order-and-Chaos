using UnityEngine;

// Class for managing rock placement detection
public class Platform : MonoBehaviour
{
    public delegate void OnActivation(bool isActivated);
    public OnActivation onActivation;
    
    protected Animator _anim;
    protected AudioSource _audio;
    
    public CharacterColour colour;
    protected bool _isActivated;
    
    protected void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
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
}
