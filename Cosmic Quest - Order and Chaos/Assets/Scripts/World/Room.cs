using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for Chaos Void rooms
public class Room : MonoBehaviour
{
    public Animator Anim;   // Door animation to be played when all puzzles have been solved
    protected Collider m_Collider;  // Collider of the door
    protected List<GameObject> m_Enemies;   // All enemies in the room
    protected Transform[] children;     // Track all child transforms of the room
    protected List<GameObject> m_Levers;    // All levers in the room
    protected List<GameObject> m_Platforms; // All rock platforms in the room
    
    void Awake ()
    {
        // Track all rock platforms in the room
        m_Platforms = new List<GameObject>();

        // Track all levers in the room
        m_Levers = new List<GameObject>();

        // Populate enemy list with enemies in the room
        m_Enemies = new List<GameObject>();

        // TODO: Find better way of tracking all enemies in a room
        children = transform.parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.gameObject.tag == "Enemy")
            {
                m_Enemies.Add(child.gameObject);
            }
            if (child.gameObject.tag == "Platform")
            {
                m_Platforms.Add(child.gameObject);
            }
            if (child.gameObject.tag == "Lever")
            {
                m_Levers.Add(child.gameObject);
            }
        }

        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();
    }

    // Returns whether all rocks have been positioned on their respective platforms
    public virtual bool ArePlatformsActivated ()
    {
        Debug.Log("Platform count = " + m_Platforms.Count);
        if (m_Platforms == null || m_Platforms.Count == 0) return true;

        // Check if every platform in the room has a rock placed on it
        foreach (GameObject plat in m_Platforms)
        {
            if (!plat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlatformActivated"))
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
        Debug.Log("Lever count = " + m_Levers.Count);
         if (m_Levers == null || m_Levers.Count == 0) return true;

        // Check if every lever has been activated
        foreach (GameObject lever in m_Levers)
        {
            Transform handle = lever.transform.Find("Handle");
            
            if (!handle.GetComponent<Animator>().GetBool("LeverPulled"))
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
        foreach (GameObject enemy in m_Enemies)
        {
            if (!enemy.GetComponent<EnemyStatsController>().isDead)
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
