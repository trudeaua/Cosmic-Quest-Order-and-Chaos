using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for the puzzle mechanisms of a dungeon room.
public class Room : MonoBehaviour
{
    public Animator Anim;
    protected Collider m_Collider;  // Collider of the door
    protected GameObject[] m_Enemies;   // All enemies in the room
    protected GameObject[] m_Levers;    // All levers in the room
    protected GameObject[] m_Platforms; // All rock platforms in the room
    
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
        bool enemiesKilled = false;

        // Check if every enemy in the room is dead
        foreach (GameObject enemy in m_Enemies)
        {
            enemiesKilled = enemy.gameObject.GetComponent<EntityStatsController>().isDead;
        }
        
        return enemiesKilled;
    }

    public virtual void PauseDoorAnimationEvent()
    {
        Anim.enabled = false;
    }
}
