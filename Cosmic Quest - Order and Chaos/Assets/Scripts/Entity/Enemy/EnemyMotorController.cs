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

    public float rotationSpeed = 1.0f;

    private void Awake()
    {
        _stats = GetComponent<EnemyStatsController>();
        _brain = GetComponent<EnemyBrainController>();
        _agent = GetComponent<NavMeshAgent>();
        
        // Set avoidance priority to random number (0-99) to prevent enemy clustering
        _agent.avoidancePriority = Random.Range(0, 100);
    }

    public void StartFollow()
    {
        _agent.isStopped = false;
        StartCoroutine(FollowTarget());
    }

    public void StopFollow()
    {
        StopCoroutine(FollowTarget());
    }

    public void StartRotate()
    {
        StartCoroutine(RotateToTarget());
    }

    public void StopRotate()
    {
        StopCoroutine(RotateToTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            _target = _brain.GetCurrentTarget();

            if (_target && _agent.enabled)
            {
                _agent.SetDestination(_target.position);
            }

            // Delay loop to increase lessen load on path finding
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RotateToTarget()
    {
        while (true)
        {
            _target = _brain.GetCurrentTarget();

            if (_target)
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
