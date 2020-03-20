using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantIdleBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        if (_brain.GetCurrentTarget())
        {
            animator.SetBool("IsAggro", true);
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
