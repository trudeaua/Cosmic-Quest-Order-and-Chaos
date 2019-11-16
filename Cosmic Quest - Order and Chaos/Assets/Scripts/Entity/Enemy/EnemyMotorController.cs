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
    private Animator _anim;
    private NavMeshAgent _agent;

    private Transform _currentTarget = null;

    private void Awake()
    {
        _stats = GetComponent<EnemyStatsController>();
        _brain = GetComponent<EnemyBrainController>();
        _anim = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Prevent enemy activity during death animation
        if (_stats.isDead)
            return;

        // Ensure current target is up to date
        _currentTarget = _brain.currentTarget;
        
        // Follow current aggro decision
        if (_currentTarget)
        {
            _agent.SetDestination(_currentTarget.position);

            float distance = (_currentTarget.position - transform.position).sqrMagnitude;
            if (distance <= _agent.stoppingDistance * _agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

        // Set walk animation
        _anim.SetBool("Walk Forward", _agent.velocity != Vector3.zero);
    }

    private void FaceTarget()
    {
        Vector3 direction = (_brain.currentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
