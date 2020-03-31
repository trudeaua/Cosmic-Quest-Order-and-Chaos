using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentIdleBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private SerpentBossCombatController _combat;
    private EnemyMotorController _motor;
    private Transform _target;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<SerpentBossCombatController>();
        _motor = animator.GetComponent<EnemyMotorController>();
        
        // Always face the target
        _motor.StartRotate();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        _target = _brain.GetCurrentTarget();

        if (_target is null)
            return;
        
        // Try to attack player if they are close enough
        if (Vector3.Distance(animator.transform.position, _target.position) <= _brain.attackRadius)
        {
            if (!_combat.IsCoolingDown)
            {
                // Go to attack state
                _combat.ChooseAttack();
                return;
            }
        }
        
        // Move to target player
        animator.SetTrigger("Follow");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _motor.StopRotate();
    }
}
