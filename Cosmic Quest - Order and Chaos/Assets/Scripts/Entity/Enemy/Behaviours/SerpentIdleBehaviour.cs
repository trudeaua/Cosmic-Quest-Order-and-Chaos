using UnityEngine;
using UnityEngine.AI;

public class SerpentIdleBehaviour : StateMachineBehaviour
{
    private EnemyBrainController _brain;
    private SerpentBossCombatController _combat;
    private NavMeshAgent _agent;
    private Transform _target;

    [Tooltip("The max angle between the look direction of the enemy and its target before rotating")]
    public float angleFromTargetTolerance = 30f;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = animator.GetComponent<EnemyBrainController>();
        _combat = animator.GetComponent<SerpentBossCombatController>();
        _agent = animator.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;
        
        _target = _brain.GetCurrentTarget();

        if (_target is null)
            return;
        
        // See if we can use our special attack
        if (_combat.CanUseSpecialAttack)
        {
            _combat.SpecialAttack();
            return;
        }

        float distance = Vector3.Distance(animator.transform.position, _target.position);
        
        // Try to attack player if they are close enough
        if (distance <= _brain.attackRadius)
        {
            // See if we need to rotate towards the player
            float angle = Vector3.SignedAngle(_target.position - animator.transform.position, animator.transform.forward, Vector3.up);
            if (Mathf.Abs(angle) > angleFromTargetTolerance)
            {
                animator.SetTrigger(angle < 0 ? "RotateLeft" : "RotateRight");
                return;
            }
            
            if (!_combat.IsCoolingDown)
            {
                // Go to attack state
                _combat.ChooseAttack();
                return;
            }
        }
        
        // Move to target player if necessary
        if (distance > _agent.stoppingDistance)
            animator.SetTrigger("Follow");
    }
}
