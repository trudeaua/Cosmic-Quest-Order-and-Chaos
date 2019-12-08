using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _velocity;
    private float _range;
    private Vector3 _initialPosition;
    private Rigidbody _rb;

    private const float ProjectileHeight = 1f;

    protected EntityStatsController LauncherStats;

    private void Awake()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if projectile has reached its maximum range
        if ((transform.position - _initialPosition).sqrMagnitude >= _range * _range)
        {
            EndLaunch();
        }
    }

    public void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range)
    {
        LauncherStats = launcherStats;
        _range = range;
        
        // Set position just in front of launcher
        _initialPosition = launcherStats.transform.position + launcherStats.transform.forward;
        _initialPosition.y = ProjectileHeight + 0.1f;
        transform.position = _initialPosition;
        
        // Set rotation to launch direction
        transform.rotation = Quaternion.LookRotation(direction);
        
        // Set self active and begin launch
        gameObject.SetActive(true);
        
        // Reset forces and apply launch force to the rigidbody
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.AddForce(launchForce * transform.forward);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + " has collided with " + other.gameObject.name);
        gameObject.SetActive(false);
    }

    protected virtual void EndLaunch()
    {
        gameObject.SetActive(false);
    }
}
