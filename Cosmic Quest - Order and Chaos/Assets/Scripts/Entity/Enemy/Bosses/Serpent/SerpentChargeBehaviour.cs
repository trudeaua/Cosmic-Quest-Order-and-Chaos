using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SerpentChargeBehaviour : StateMachineBehaviour
{
    public float chargeSpeed;
    public string exitTrigger;
    
    private float _originalSpeed;
    private EnemyBrainController _brain;
    private SerpentBossCombatController _combat;
    private EnemyMotorController _motor;
    private NavMeshAgent _agent;
    
    private Transform _target;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<SerpentBossCombatController>();
        _motor = animator.GetComponent<EnemyMotorController>();
        _agent = animator.GetComponent<NavMeshAgent>();

        _originalSpeed = _agent.speed;
        _agent.speed = chargeSpeed;
        
        _target = _brain.GetCurrentTarget();
        _motor.StartFollow();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        // Check if we run into the rock we threw
        
        // Check if we run into any other players along the way
        foreach (GameObject player in PlayerManager.Instance.AlivePlayers)
        {
            if (_combat.CanDamageTarget(player, _brain.attackRadius, 180f))
            {
                animator.SetTrigger(exitTrigger);
            }
        }
        
        // Ensure that players are alive
        if (PlayerManager.Instance.NumPlayersAlive() == 0)
            animator.SetTrigger("CancelAction");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _motor.StartFollow();
        _agent.speed = _originalSpeed;
    }
}
