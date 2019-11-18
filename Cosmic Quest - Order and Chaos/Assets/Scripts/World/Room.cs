using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Animator Anim;

    public virtual bool RocksPositioned ()
    {
        return false;
    }

    public virtual bool LeversActivated ()
    {
        return false;
    }

    public virtual void PauseDoorAnimationEvent()
    {
        Anim.enabled = false;
    }
}
