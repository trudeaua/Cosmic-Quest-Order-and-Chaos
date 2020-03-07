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

    private void Awake()
    {
        _stats = GetComponent<EnemyStatsController>();
        _brain = GetComponent<EnemyBrainController>();
        _agent = GetComponent<NavMeshAgent>();
        
        // Set avoidance priority to random number (0-99)
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

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            _target = _brain.GetCurrentTarget();

            if (_target && _agent.enabled)
            {
                _agent.SetDestination(_target.position);
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    /*private void Update()
    {
        // Prevent enemy activity during death animation
        if (_stats.isDead)
            return;

        // Ensure current target is up to date
        _currentTarget = _brain.GetCurrentTarget();
        
        // Follow current aggro decision
        if (_currentTarget && _agent.enabled)
        {
            _agent.SetDestination(_currentTarget.position);

            float distance = (_currentTarget.position - transform.position).sqrMagnitude;
            if (distance <= _agent.stoppingDistance * _agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

        // Trigger walking animation
        _anim.SetFloat("WalkSpeed", _agent.velocity.magnitude);
    }*/
    
    /// <summary>
    /// Rotate the enemy to face the current target
    /// </summary>
    private void FaceTarget()
    {
        if (_target is null)
            return;
        
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
