using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhakChargeBehaviour : StateMachineBehaviour
{
    private RhakCombatController _combat;
    private EnemyBrainController _brain;
    private int _numCharges;
    private int _totalCharges;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combat = animator.GetComponent<RhakCombatController>();
        _brain = animator.GetComponent<EnemyBrainController>();

        // Decide the number of charges to perform
        _totalCharges = Random.Range(1, _combat.maxConsecutiveCharges);
        _numCharges = 0;
        
        // Start the charge attack
        Transform target = _brain.GetRandomTarget();
        _combat.StartCharge(target.position);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        // Wait for current charge attack to be finished
        if (_combat.IsCharging)
            return;
        
        // Decide if we perform another charge attack
        _numCharges += 1;
        if (_numCharges < _totalCharges)
        {
            // Select a new target and charge them
            Transform target = _brain.GetRandomTarget();
            _combat.StartCharge(target.position);
        }
        else
        {
            animator.SetBool("Charging", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
