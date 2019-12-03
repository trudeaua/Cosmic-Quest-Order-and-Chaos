using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStatsController))]
public class EnemyBrainController : MonoBehaviour
{
    private class TargetPlayer
    {
        public GameObject Player;
        public EntityStatsController Stats;
        public float Aggro = 0f;
        public bool IsKnown = false;
    }
    
    [Tooltip("The radius around the enemy where a player can trigger aggro")]
    public float aggroRadius = 10f;
    [Tooltip("The time in seconds between decisions on which player to aggro")]
    public float decisionDelay = 0.5f;

    private TargetPlayer _currentTarget;
    
    private EnemyStatsController _stats;
    private EnemyCombatController _combat;
    private List<TargetPlayer> _targets;

    public bool IsStunned { get; private set; }
    private float _decisionTimer = 0f;

    private void Awake()
    {
        _stats = GetComponent<EnemyStatsController>();
        _combat = GetComponent<EnemyCombatController>();
    }

    private void Start()
    {
        // Initialize target array
        _targets = new List<TargetPlayer>();
        foreach (GameObject player in PlayerManager.Players)
        {
            TargetPlayer target = new TargetPlayer {Player = player, Stats = player.GetComponent<EntityStatsController>()};
            _targets.Add(target);
        }
    }

    private void Update()
    {
        // Prevent enemy activity during death animation or while stunned
        if (_stats.isDead || IsStunned)
            return;
        
        // Update list of targets
        UpdateTargetList();
        
        // Make any combat decisions
        MakeCombatDecision();
        
        _decisionTimer -= Time.deltaTime;
        if (_decisionTimer > 0f)
            return;

        _decisionTimer = decisionDelay;
        
        // Make decision about which player to aggro
        MakeTargetDecision();
    }
    
    private void UpdateTargetList()
    {
        foreach (TargetPlayer target in _targets)
        {
            if (CanSee(target))
            {
                // Set player to known state
                target.IsKnown = true;
            }
            else if (target.IsKnown)
            {
                // Reset player targets state
                target.IsKnown = false;
                target.Aggro = 0f;
            }
        }
    }

    private void MakeTargetDecision()
    {
        // 1. Prioritize top aggro value
        TargetPlayer newTarget = GetTopAggroTarget();
        if (newTarget != null)
        {
            _currentTarget = newTarget;
            return;
        }
        
        // 2. Choose closest player
        newTarget = GetClosestTarget();
        if (newTarget != null)
        {
            _currentTarget = newTarget;
            return;
        }

        // 3. No target
        _currentTarget = null;
    }

    private void MakeCombatDecision()
    {
        if (_currentTarget is null || _combat is null)
            return;

        // Choose a new target if the current one is now dead
        if (_currentTarget.Stats.isDead)
        {
            MakeTargetDecision();
            if (_currentTarget is null)
                return;
        }
        
        float distance = (_currentTarget.Player.transform.position - transform.position).sqrMagnitude;
        if (distance <= _combat.attackRadius * _combat.attackRadius)
        {
            _combat.PrimaryAttack();
        }
    }

    private TargetPlayer GetTopAggroTarget()
    {
        TargetPlayer targetPlayer = null;
        float maxAggro = 0f;
        
        foreach (TargetPlayer target in _targets)
        {
            // Ensure the target is known, and has an aggro value over zero
            if (target.IsKnown && target.Aggro > maxAggro && CanSee(target))
            {
                maxAggro = target.Aggro;
                targetPlayer = target;
            }
        }

        return targetPlayer;
    }

    private TargetPlayer GetClosestTarget()
    {
        TargetPlayer targetPlayer = null;
        float minDist = Mathf.Infinity;

        foreach (TargetPlayer target in _targets)
        {
            float distance = (target.Player.transform.position - transform.position).sqrMagnitude;
            if (target.IsKnown && distance < minDist && CanSee(target))
            {
                minDist = distance;
                targetPlayer = target;
            }
        }

        return targetPlayer;
    }

    private bool CanSee(TargetPlayer target)
    {
        if (Physics.Linecast(transform.position + Vector3.up, target.Player.transform.position + Vector3.up, out RaycastHit hit))
        {
            return (target.Player.transform.position - transform.position).sqrMagnitude <= aggroRadius * aggroRadius && hit.transform.gameObject == target.Player;
        }
        return false;
    }

    public Transform GetCurrentTarget()
    {
        return _currentTarget?.Player.transform;
    }
    
    public void OnDamageTaken(GameObject player, float damageAmount)
    {
        // Update aggro value with amount of damage taken
        TargetPlayer target = _targets.Find(t => t.Player == player);
        
        target.Aggro += damageAmount;
        target.IsKnown = true;
    }

    public void SetStunned(bool isStunned)
    {
        IsStunned = isStunned;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the aggro radius of the enemy in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
