using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SerpentChargeBehaviour : StateMachineBehaviour
{
    public string exitTrigger;
    
    private EnemyBrainController _brain;
    private SerpentBossCombatController _combat;
    private EnemyMotorController _motor;
    private NavMeshAgent _agent;
    private Transform _target;
    
    private float _originalSpeed;
    private float _originalStoppingDistance;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<SerpentBossCombatController>();
        _motor = animator.GetComponent<EnemyMotorController>();
        _agent = animator.GetComponent<NavMeshAgent>();

        _originalSpeed = _agent.speed;
        _originalStoppingDistance = _agent.stoppingDistance;
        _agent.speed = _combat.chargeSpeed;
        _agent.stoppingDistance = _combat.chargedAttackTriggerDistance;
        
        // Select a random target to charge
        _target = _brain.GetRandomTarget();
        _motor.StartFollow(_target);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        // Ensure that the target is alive
        if (PlayerManager.Instance.NumPlayersAlive() == 0 ||
            !PlayerManager.Instance.AlivePlayers.Contains(_target.gameObject))
            animator.SetTrigger("CancelAction");
        
        // Check if we run into any other players along the way
        foreach (GameObject player in PlayerManager.Instance.AlivePlayers)
        {
            if (_combat.CanDamageTarget(player, _combat.chargedAttackTriggerDistance, 120f))
            {
                animator.SetTrigger(exitTrigger);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _motor.StopFollow();
        _agent.speed = _originalSpeed;
        _agent.stoppingDistance = _originalStoppingDistance;
    }
}
