using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAggroBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private PlantCombatController _combat;
    private EnemyMotorController _motor;
    private Transform _target;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<PlantCombatController>();
        _motor = animator.GetComponent<EnemyMotorController>();
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
        
        // Always face player if they are not in the field of attack
        if (Vector3.Angle(_target.position - animator.transform.position, animator.transform.forward) > 10f && !_motor.IsRotating)
        {
            _motor.StartRotate();
        }
        else if (_motor.IsRotating)
        {
            _motor.StopRotate();
        }
        
        // Try to attack player
        if (!_combat.IsCoolingDown)
        {
            _combat.ChooseAttack();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
