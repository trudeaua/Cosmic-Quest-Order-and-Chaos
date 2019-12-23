using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Base class for Chaos Void rooms.
// Should be attached to door of the room.
// Make sure door is under a game object "Room" so the script can properly search all objects in the room.
public class Room : MonoBehaviour
{
    public Animator Anim;   // Door animation to be played when all puzzles have been solved
    public Collider Collider;  // Collider of the door
    public GameObject Door;     
    public AudioSource DoorAudio; // Door open/close audio clip

    private Transform[] _children;     // Track all child transforms in the room
    private List<EnemyStatsController> _enemies;   // All enemies in the room
    private List<Lever> _levers;    // All levers in the room
    private List<Platform> _platforms; // All rock platforms in the room

    // TODO: Implement random generator for lever code patterns based on input of code length and active player colours
    // For pattern-based puzzles
    public List<CharacterColour> Code;  // List containing input code sequence 
    public List<CharacterColour> Input; // Player input that'll be checked against stored code
    
    private void Awake ()
    {
        // Track all rock platforms in the room
        _platforms = new List<Platform>();

        // Track all levers in the room
        _levers = new List<Lever>();

        // Populate enemy list with enemies in the room
        _enemies = new List<EnemyStatsController>();

        // // TODO: Implement random generator for lever code patterns based on input of code length and active player colours
        // Code = new List<CharacterColour>();
        // Input = new List<CharacterColour>();

        _children = transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in _children)
        {
            GameObject obj = child.gameObject;

            if (obj.GetComponent<EnemyStatsController>())
            {
                _enemies.Add(obj.GetComponent<EnemyStatsController>());
            }
            else if (obj.GetComponent<Platform>())
            {
                _platforms.Add(obj.GetComponent<Platform>());
            }
            else if (obj.GetComponent<Lever>())
            {
                _levers.Add(obj.GetComponent<Lever>());
            }
        }

        Debug.Log("Enemy count = " + _enemies.Count);
        Debug.Log("Platform count = " + _platforms.Count);
    }

    private void Start()
    {
        Door = transform.Find("Door").gameObject;

        Collider = Door.GetComponent<Collider>();
        Anim = Door.GetComponent<Animator>();
        DoorAudio = Door.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Returns whether all platforms in the room have been activated.<br/>
    /// </summary>
    /// <returns>Bool for whether all platforms are activated or not.</returns>
    public virtual bool ArePlatformsActivated ()
    {
        if (_platforms == null || _platforms.Count == 0) return true;

        // Check if every platform in the room has a rock placed on it
        foreach (Platform plat in _platforms)
        {
            if (!plat.IsActivated)
            {
                // If a platform hasn't been activated yet, return false
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns whether all levers in the room have been pulled.<br/>
    /// </summary>
    /// <returns>Bool for whether all levers have been pulled or not.</returns>
    public virtual bool AreLeversPulled ()
    {
        if (_levers == null || _levers.Count == 0) return true;

        // Check if every lever has been activated
        foreach (Lever lever in _levers)
        {
            if (!lever.IsPulled)
            {
                // If at least 1 lever isn't activated, return false
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns whether all enemies in the room have been killed.<br/>
    /// </summary>
    /// <returns>Bool for whether all enemies have been killed.</returns>
    public virtual bool AreAllEnemiesKilled ()
    {
        // If enemy list is empty, all enemies in the room have been killed
        if (_enemies.Count == 0) return true;

        // Check for dead enemies and remove them from the enemy list
        foreach (EnemyStatsController enemy in _enemies)
        {
            if (!enemy.isDead)
            {
                return false;
            }
        }

        return true;
    }

    public virtual void PauseAnimEvent()
    {
        Anim.enabled = false;
    }

    public virtual IEnumerator SetAnimTrigger ()
    {
        DoorAudio.Play(0);
        yield return new WaitForSeconds(1);
        Anim.SetTrigger("OpenDoor");
        Collider.enabled = false;

        yield break;
    }
}
