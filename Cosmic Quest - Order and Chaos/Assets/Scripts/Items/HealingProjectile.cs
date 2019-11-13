using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingProjectile : Projectile
{
    
    
    protected override void OnCollisionEnter(Collision collision)
    {
        // Collision event
        Debug.Log(name + " has collided with " + collision);
    }
}
