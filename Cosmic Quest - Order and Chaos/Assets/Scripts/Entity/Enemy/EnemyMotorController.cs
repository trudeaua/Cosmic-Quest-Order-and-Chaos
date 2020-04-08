using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyBrainController))]
[RequireComponent(typeof(EnemyStatsController))]
public class EnemyMotorController : MonoBehaviour
{
    private EnemyStatsController _stats;
    private EnemyBrainController _brain;
    private NavMeshAgent _agent;

    private Transform _target;
    private bool _ignoreBrain;

    public float rotationSpeed = 1.0f;

    public bool IsFollowing { get; private set; }
    public bool IsRotating { get; private set; }
    
    private void Awake()
    {
        _stats = GetComponent<EnemyStatsController>();
        _brain = GetComponent<EnemyBrainController>();
        _agent = GetComponent<NavMeshAgent>();
        
        // Set avoidance priority to random number (0-99) to prevent enemy clustering
        _agent.avoidancePriority = Random.Range(0, 100);
    }

    public void StartFollow(Transform staticTarget = null)
    {
        if (staticTarget)
        {
            _ignoreBrain = true;
            _target = staticTarget;
        }
        else
        {
            _ignoreBrain = false;
        }
        
        _agent.isStopped = false;
        IsFollowing = true;
        StartCoroutine(FollowTarget());
    }

    public void StopFollow()
    {
        _agent.isStopped = true;
        IsFollowing = false;
        StopCoroutine(FollowTarget());
    }

    public void StartRotate()
    {
        IsRotating = true;
        StartCoroutine(RotateToTarget());
    }

    public void StopRotate()
    {
        IsRotating = false;
        StopCoroutine(RotateToTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (IsFollowing)
        {
            if (!_ignoreBrain)
                _target = _brain.GetCurrentTarget();

            if (_target && _agent.enabled && !_stats.isDead)
            {
                _agent.SetDestination(_target.position);
            }

            // Delay loop to increase lessen load on path finding
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RotateToTarget()
    {
        while (IsRotating)
        {
            _target = _brain.GetCurrentTarget();

            if (_target && !_stats.isDead)
            {
                // Rotate enemy to face target
                Vector3 direction = (_target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
            
            // Wait for end of frame
            yield return null;
        }
    }
}
