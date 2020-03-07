using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private EnemyCombatController _combat;
    private EnemyMotorController _motor;
    private NavMeshAgent _agent;
    private Transform _target;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<EnemyCombatController>();
        _motor = animator.GetComponent<EnemyMotorController>();
        _agent = animator.GetComponent<NavMeshAgent>();

        _motor.StartFollow();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _target = _brain.GetCurrentTarget();

        if (_target is null)
        {
            animator.SetTrigger("Idle");
            return;
        }

        if (_agent.enabled)
        {
            if (Vector3.Distance(_agent.destination, animator.transform.position) <= _combat.attackRadius)
            {
                // Go to attack state
                animator.SetTrigger("PrimaryAttack");
                _agent.isStopped = true;
            }
        }
        
        animator.SetFloat("WalkSpeed", _agent.velocity.magnitude);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("PrimaryAttack");
        _motor.StopFollow();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
