using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private EnemyCombatController _combat;
    private Transform _target;

    public bool canFollow = true;
    public bool canPatrol;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<EnemyCombatController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(0))
            return;
        
        _target = _brain.GetCurrentTarget();

        if (!_target)
            return;
        
        // Try to attack player if they are close enough
        if (Vector3.Distance(animator.transform.position, _target.position) <= _brain.attackRadius)
        {
            // Go to attack state
            _combat.ChooseAttack();
            return;
        }
        
        // Follow player if possible
        if (canFollow)
        {
            animator.SetTrigger("Follow");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
