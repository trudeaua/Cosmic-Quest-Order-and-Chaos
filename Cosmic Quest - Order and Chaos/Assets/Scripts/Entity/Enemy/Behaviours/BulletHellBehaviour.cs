using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellBehaviour : StateMachineBehaviour
{
    private RhakCombatController _combat;
    private RhakStatsController _stats;
    private EnemyMotorController _motor;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combat = animator.GetComponent<RhakCombatController>();
        _stats = animator.GetComponent<RhakStatsController>();
        _motor = animator.GetComponent<EnemyMotorController>();
        
        _combat.StartBulletHell();
        _motor.StartRotate();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        // If all orbs have hit Rhak, then transition to phase 2
        if (_stats.OrbsRequired.Count == 0)
        {
            animator.SetBool("Phase 1", false);
            animator.SetBool("Phase 2", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combat.StopBulletHell();
        _motor.StopRotate();
    }
}
