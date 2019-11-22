using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for the puzzle mechanisms of a dungeon room.
public class Room : MonoBehaviour
{
    public Animator Anim;
    protected Collider m_Collider;  // Collider of the door
    protected List<GameObject> m_Enemies;   // All enemies in the room
    protected GameObject[] m_Levers;    // All levers in the room
    protected GameObject[] m_Platforms; // All rock platforms in the room
    
    void Awake ()
    {
        // Track all rock platforms in the room
        m_Platforms = GameObject.FindGameObjectsWithTag("Platform");

        // Track all levers in the room
        m_Levers = GameObject.FindGameObjectsWithTag("Lever");

        // Populate enemy list with enemies in the room
        m_Enemies = new List<GameObject>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            m_Enemies.Add(enemy);
        }

        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();
    }

    public virtual bool AreRocksPositioned ()
    {
        bool rocksPositioned = true;

        // Check if every platform has a rock placed on it
        foreach (GameObject plat in m_Platforms)
        {
            if (!plat.GetComponent<Animator>().GetBool("RockPlaced"))
            {
                // If at least 1 platform does not have a rock placed, return false
                rocksPositioned = false;
            }
        }

        return rocksPositioned;
    }

    public virtual bool AreLeversActivated ()
    {
        bool leversActivated = true;

        // Check if every lever has been activated
        foreach (GameObject lever in m_Levers)
        {
            Transform handle = lever.transform.Find("Handle");
            
            if (!handle.GetComponent<Animator>().GetBool("LeverActivated"))
            {
                // If at least 1 lever isn't activated, return false
                leversActivated = false;
            }
        }

        return leversActivated;
    }

    public virtual bool AreAllEnemiesKilled ()
    {
        // Check for dead enemies and remove them from the enemy list
        foreach (GameObject enemy in m_Enemies)
        {
            if (enemy != null && enemy.GetComponent<EnemyStatsController>().isDead == true)
            {
                m_Enemies.Remove(enemy);
            }
        }
        
        // If enemy list is empty, all enemies in the room have been killed
        if (m_Enemies.Count == 0)
        {   
            return true;
        }

        return false;
    }

    public virtual void PauseDoorAnimEvent()
    {
        Anim.enabled = false;
    }

    public IEnumerator SetDoorAnimTrigger ()
    {
        yield return new WaitForSeconds(1f);
        Anim.SetTrigger("OpenDoor");
        m_Collider.enabled = false;
        yield break;
    }
}
