using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTransitionBehaviour : StateMachineBehaviour
{
    private RhakStatsController _stats;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stats = animator.GetComponent<RhakStatsController>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Remove invincibility for second phase
        _stats.invincible = false;
    }
}
