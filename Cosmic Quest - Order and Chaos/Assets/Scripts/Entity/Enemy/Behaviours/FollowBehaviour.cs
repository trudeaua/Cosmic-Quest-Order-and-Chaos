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
        if (animator.IsInTransition(layerIndex))
            return;
        
        _target = _brain.GetCurrentTarget();

        if (_target is null && !_agent.hasPath)
        {
            animator.SetTrigger("Idle");
            return;
        }

        if (_agent.enabled && _target)
        {
            // See if we can use our special attack
            if (_combat.CanUseSpecialAttack)
            {
                _combat.SpecialAttack();
                return;
            }
            
            if (!_combat.IsCoolingDown && Vector3.Distance(_target.position, animator.transform.position) <= _brain.attackRadius)
            {
                // Go to attack state
                bool choseAttack = _combat.ChooseAttack();
                
                if (choseAttack)
                    return;
            }
        }
        
        animator.SetFloat("WalkSpeed", _agent.velocity.magnitude);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _motor.StopFollow();
    }
}
