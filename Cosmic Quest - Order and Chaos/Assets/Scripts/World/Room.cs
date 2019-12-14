using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Base class for Chaos Void rooms
public class Room : MonoBehaviour
{
    public Animator Anim;   // Door animation to be played when all puzzles have been solved
    protected Collider m_Collider;  // Collider of the door

    protected Transform[] children;     // Track all child transforms of the room
    protected List<EnemyStatsController> m_Enemies;   // All enemies in the room
    protected List<Lever> m_Levers;    // All levers in the room
    protected List<Platform> m_Platforms; // All rock platforms in the room

    // TODO: Implement random generator for lever code patterns based on input of code length and active player colours
    // For pattern-based puzzles
    protected List<CharacterColour> code;  // List containing input code sequence 
    public List<CharacterColour> input; // Player input that'll be checked against stored code
    
    void Awake ()
    {
        // Track all rock platforms in the room
        m_Platforms = new List<Platform>();

        // Track all levers in the room
        m_Levers = new List<Lever>();

        // Populate enemy list with enemies in the room
        m_Enemies = new List<EnemyStatsController>();

        // TODO: Find better way of tracking all enemies in a room
        children = transform.parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            GameObject obj = child.gameObject;

            if (obj.GetComponent<EnemyStatsController>())
            {
                m_Enemies.Add(obj.GetComponent<EnemyStatsController>());
            }
            else if (obj.GetComponent<Platform>())
            {
                m_Platforms.Add(obj.GetComponent<Platform>());
            }
            else if (obj.GetComponent<Lever>())
            {
                m_Levers.Add(obj.GetComponent<Lever>());
            }
        }

        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();
    }

    // Returns whether all rocks have been positioned on their respective platforms
    public virtual bool ArePlatformsActivated ()
    {
        if (m_Platforms == null || m_Platforms.Count == 0) return true;

        // Check if every platform in the room has a rock placed on it
        foreach (Platform plat in m_Platforms)
        {
            if (!plat.IsActivated)
            {
                // If a platform hasn't been activated yet, return false
                return false;
            }
        }

        return true;
    }

    // Returns whether all levers in the room have been pulled
    public virtual bool AreLeversPulled ()
    {
        if (m_Levers == null || m_Levers.Count == 0) return true;

        // Check if every lever has been activated
        foreach (Lever lever in m_Levers)
        {
            if (!lever.IsPulled)
            {
                // If at least 1 lever isn't activated, return false
                return false;
            }
        }

        return true;
    }

    // Returns whether all enemies in the room have been killed
    public virtual bool AreAllEnemiesKilled ()
    {
        // If enemy list is empty, all enemies in the room have been killed
        if (m_Enemies.Count == 0) return true;

        // Check for dead enemies and remove them from the enemy list
        foreach (EnemyStatsController enemy in m_Enemies)
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

        yield return new WaitForSeconds(1);
        Anim.SetTrigger("OpenDoor");
        m_Collider.enabled = false;
        yield break;
    }
}
