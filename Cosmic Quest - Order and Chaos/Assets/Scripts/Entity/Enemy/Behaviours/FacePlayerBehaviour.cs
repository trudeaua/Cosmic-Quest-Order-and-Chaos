using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private EnemyMotorController _motor;
    private Transform _target;

    private float _angle;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _motor = animator.GetComponent<EnemyMotorController>();
        
        _motor.StartRotate();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        _target = _brain.GetCurrentTarget();

        if (_target)
        {
            _angle = Vector3.SignedAngle(_target.position - animator.transform.position, animator.transform.forward, Vector3.up);
        }
        
        // If there is no target within reach, or within 30 degree view of target, exit state
        if (_target is null
            || Vector3.Distance(_target.position, animator.transform.position) > _brain.attackRadius
            || Mathf.Abs(_angle) < 15f)
        {
            animator.SetTrigger("Idle");
        }
        
        animator.SetFloat("RotationSpeed", _angle < 0f ? -1f : 1f);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _motor.StopRotate();
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
