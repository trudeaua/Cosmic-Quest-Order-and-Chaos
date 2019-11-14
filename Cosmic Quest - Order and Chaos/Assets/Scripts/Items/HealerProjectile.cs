using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerProjectile : Projectile
{
    protected override void OnCollision(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!");
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player!");
        }
        
        gameObject.SetActive(false);
    }
}
