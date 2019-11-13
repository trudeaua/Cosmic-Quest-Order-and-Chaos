using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // TODO Perhaps define this in the Launch arguments?
    [Tooltip("The maximum range that the projectile can travel")]
    public float range = 20f;
    [Tooltip("The velocity the projectile will travel at")]
    public float velocity = 5f;

    private bool _launched;
    private Vector3 _initialPosition;
    
    private void Start()
    {
        _launched = false;
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (_launched)
        {
            transform.Translate(Time.deltaTime * velocity * transform.forward);
            
            // Check for any collisions
            
            // Check if projectile has reached its maximum range
            if ((transform.position - _initialPosition).sqrMagnitude >= range * range)
            {
                // Despawn
            }
        }
    }

    private void Reset()
    {
        // Reset projectile
    }

    public void Launch(Transform launcher, Vector3 direction)
    {
        // Look in direction and launch
    }

    protected virtual void OnCollision(Transform hit)
    {
        // Collision event
    }
}
