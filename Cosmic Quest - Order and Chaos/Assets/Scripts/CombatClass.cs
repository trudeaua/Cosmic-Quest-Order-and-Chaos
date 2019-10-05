using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for combat classes to extend from
public class CombatClass : MonoBehaviour
{
    public string className;
    public Stat[] statModifiers;

    public virtual void PrimaryAttack()
    {
        // TODO
    }

    public virtual void SecondaryAttack()
    {
        // TODO
    }
}
