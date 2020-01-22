using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, ISerializable
{
    private AudioSource _audio;
    private Animator _anim;
    private BoxCollider _col;

    public Puzzle puzzle;
    public bool isOpen { get; private set; }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _col = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        // Subscribe the door to open when its puzzle is complete
        puzzle.onCompletion += Open;
    }

    private void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine("OpenDoor");
        }
    }

    private IEnumerator OpenDoor()
    {
        // Play door opening audio clip
        if (_audio)
            _audio.PlayDelayed(0f);
        
        // Play door opening animation
        if (_anim)
            _anim.SetBool("UnlockDoor", true);

        // Wait until the animation completes
        yield return new WaitForSeconds(1f);
        
        // Disable the door collider
        _col.enabled = false;
    }

    public string Serialize()
    {
        throw new System.NotImplementedException();
    }

    public void FromSerialized(string s)
    {
        throw new System.NotImplementedException();
    }
}
