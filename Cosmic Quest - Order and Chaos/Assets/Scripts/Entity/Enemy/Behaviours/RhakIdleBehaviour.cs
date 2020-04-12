using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhakIdleBehaviour : StateMachineBehaviour
{
    private RhakCombatController _combat;
    private float _idleTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combat = animator.GetComponent<RhakCombatController>();

        _idleTimer = Random.Range(_combat.cooldownMinTime, _combat.cooldownMaxTime);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        // Trigger charge after a random wait time
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0f)
            animator.SetBool("Charging", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
