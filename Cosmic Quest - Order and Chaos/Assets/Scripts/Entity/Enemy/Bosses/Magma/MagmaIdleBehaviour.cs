using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaIdleBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private EnemyCombatController _combat;
    private Transform _target; 

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<EnemyCombatController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        _target = _brain.GetCurrentTarget();

        if (_target is null)
            return;
        
        // See if we can use our special attack
        if (_combat.CanUseSpecialAttack)
        {
            _combat.SpecialAttack();
            return;
        }
        
        // Try to attack player if they are close enough
        if (Vector3.Distance(animator.transform.position, _target.position) <= _brain.attackRadius)
        {
            if (Vector3.Angle(_target.position - animator.transform.position, animator.transform.forward) > 15f)
            {
                animator.SetTrigger("Rotate");
                return;
            }

            if (!_combat.IsCoolingDown)
            {
                // Go to attack state
                _combat.ChooseAttack();
                return;
            }
        }
        
        // Follow player if possible
        animator.SetTrigger("Follow");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
