using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _velocity;
    private float _range;
    private Vector3 _initialPosition;
    private Rigidbody _rb;

    public float launchHeight = 1.1f;

    protected EntityStatsController LauncherStats;

    private void Awake()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if projectile has reached its maximum range or fell too low
        if ((transform.position - _initialPosition).sqrMagnitude >= _range * _range || transform.position.y < -10f)
        {
            EndLaunch();
        }
    }

    /// <summary>
    /// Reset movement of rigidbody on enable
    /// </summary>
    private void OnEnable()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Launch the damage projectile
    /// </summary>
    /// <param name="launcherStats">Stats of the entity that launched the projectile</param>
    /// <param name="direction">Direction to launch in</param>
    /// <param name="launchForce">Force to apply to the projectile upon launch</param>
    /// <param name="range">Maximum range that the projectile can fly</param>
    public void Launch(EntityStatsController launcherStats, Vector3 direction, float launchForce, float range)
    {
        LauncherStats = launcherStats;
        _range = range;
        
        // Set position just in front of launcher
        _initialPosition = launcherStats.transform.position + launcherStats.transform.forward;
        _initialPosition.y = launchHeight;
        transform.position = _initialPosition;
        
        // Set rotation to launch direction
        transform.rotation = Quaternion.LookRotation(direction);
        
        // Set self active and begin launch
        gameObject.SetActive(true);
        
        // Apply launch force to the rigidbody
        _rb.AddForce(launchForce * transform.forward);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + " has collided with " + other.gameObject.name);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// End the projectile's launch
    /// </summary>
    protected virtual void EndLaunch()
    {
        gameObject.SetActive(false);
    }
}
