using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTransitionBehaviour : StateMachineBehaviour
{
    private RhakStatsController _stats;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stats = animator.GetComponent<RhakStatsController>();
        
        // Trigger dialogue or something
        animator.SetTrigger("Idle");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Remove invincibility for second phase
        _stats.invincible = false;
    }
}
