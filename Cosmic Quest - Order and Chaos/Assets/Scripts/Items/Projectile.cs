using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _velocity;
    private float _range;
    private Vector3 _initialPosition;

    private const float ProjectileHeight = 1f;

    protected EntityStatsController LauncherStats;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(_velocity * Time.deltaTime * transform.forward, Space.World);
        
        // Check if projectile has reached its maximum range
        if ((transform.position - _initialPosition).sqrMagnitude >= _range * _range)
        {
            EndLaunch();
        }
    }

    public void Launch(EntityStatsController launcherStats, Vector3 direction, float velocity, float range)
    {
        LauncherStats = launcherStats;
        _velocity = velocity;
        _range = range;
        
        // Set position just in front of launcher
        _initialPosition = launcherStats.transform.position + launcherStats.transform.forward;
        _initialPosition.y = ProjectileHeight;
        transform.position = _initialPosition;
        
        // Set rotation to launch direction
        transform.rotation = Quaternion.LookRotation(direction);
        
        // Set self active and begin launch
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Collision event
        OnCollision(other);
    }

    protected virtual void OnCollision(Collider col)
    {
        Debug.Log(name + " has collided with " + col.gameObject.name);
        gameObject.SetActive(false);
    }

    protected virtual void EndLaunch()
    {
        gameObject.SetActive(false);
    }
}
