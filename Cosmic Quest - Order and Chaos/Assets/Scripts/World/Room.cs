using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for the puzzle mechanisms of a dungeon room.
public class Room : MonoBehaviour
{
    public Animator Anim;
    protected Collider m_Collider;

    
    public virtual bool AreRocksPositioned ()
    {
        return false;
    }

    public virtual bool AreLeversActivated ()
    {
        return false;
    }

    public virtual void PauseDoorAnimationEvent()
    {
        Anim.enabled = false;
    }
}
