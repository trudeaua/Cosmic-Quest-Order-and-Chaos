using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAggroBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private PlantCombatController _combat;
    private Transform _target;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<PlantCombatController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        _target = _brain.GetCurrentTarget();

        if (_target is null)
        {
            // No target player, return to idle
            animator.SetBool("IsAggro", false);
            return;
        }
        
        // Try to attack player if they are close enough
        // This is where attack decisions are made for the plants
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
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
