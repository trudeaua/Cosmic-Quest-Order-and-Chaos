using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _velocity;
    private float _range;
    
    private bool _launched;
    private Vector3 _initialPosition;

    private const float ProjectileHeight = 1f;
    
    private void Start()
    {
        _launched = false;
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (!_launched)
            return;
        
        // Move the projectile
        transform.Translate(Time.deltaTime * _velocity * transform.forward);
        
        // Check for any collisions
        
        // Check if projectile has reached its maximum range
        if ((transform.position - _initialPosition).sqrMagnitude >= _range * _range)
        {
            EndLaunch();
        }
    }

    public void Launch(Transform launcher, Vector3 direction, float velocity, float range)
    {
        _velocity = velocity;
        _range = range;
        
        // Set position just in front of launcher
        _initialPosition = launcher.position + launcher.forward;
        _initialPosition.y = ProjectileHeight;
        transform.position = _initialPosition;
        
        // Set rotation to launch direction
        transform.rotation = Quaternion.LookRotation(direction);
        
        // Set self active and begin launch
        gameObject.SetActive(true);
        _launched = true;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Collision event
        Debug.Log(name + " has collided with " + collision);
    }

    protected virtual void EndLaunch()
    {
        _launched = false;
        gameObject.SetActive(false);
    }
}
