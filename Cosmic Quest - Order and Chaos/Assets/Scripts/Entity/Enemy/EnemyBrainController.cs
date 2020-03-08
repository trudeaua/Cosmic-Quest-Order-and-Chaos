using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyStatsController))]
public class EnemyBrainController : MonoBehaviour
{
    /// <summary>
    /// Contains information about which player is being targeted
    /// </summary>
    private class TargetPlayer
    {
        public GameObject Player;
        public EntityStatsController Stats;
        public float Aggro;
        public bool IsKnown;
    }
    
    [Tooltip("The radius around the enemy where a player can trigger aggro")]
    public float aggroRadius = 10f;
    [Tooltip("The distance from a player when the enemy will attempt to attack")]
    public float attackRadius = 4f;
    [Tooltip("The time in seconds between decisions on which player to aggro")]
    public float decisionDelay = 0.5f;

    private TargetPlayer _currentTarget;
    
    private EnemyStatsController _stats;
    private EnemyCombatController _combat;
    private List<TargetPlayer> _targets;

    public bool IsStunned { get; private set; }
    private float _decisionTimer;

    private void Awake()
    {
        _stats = GetComponent<EnemyStatsController>();
        _combat = GetComponent<EnemyCombatController>();
    }

    private void Start()
    {
        // Initialize target array
        _targets = new List<TargetPlayer>();
        foreach (GameObject player in PlayerManager.Instance.Players)
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
        //MakeCombatDecision();
        
        _decisionTimer -= Time.deltaTime;
        if (_decisionTimer > 0f)
            return;

        _decisionTimer = decisionDelay;
        
        // Make decision about which player to aggro
        MakeTargetDecision();
    }
    
    /// <summary>
    /// Update the enemy's target list
    /// </summary>
    private void UpdateTargetList()
    {
        // Check if any players are missing from list
        foreach (GameObject player in PlayerManager.Instance.Players)
        {
            if (_targets.Count(p => p.Player == player) == 0)
            {
                TargetPlayer target = new TargetPlayer {Player = player, Stats = player.GetComponent<EntityStatsController>()};
                _targets.Add(target);
            }
        }
        
        foreach (TargetPlayer target in _targets)
        {
            if (CanSee(target))
            {
                // Set player to known state
                target.IsKnown = true;
            }
            else if (target.IsKnown)
            {
                // Reset player target's state if out of sight
                target.IsKnown = false;
                target.Aggro = 0f;
            }
        }
    }
    
    /// <summary>
    /// Decide which player to target
    /// </summary>
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

    /// <summary>
    /// Update the target player by prioritizing any attacking players
    /// </summary>
    /// <returns>A target player object instance</returns>
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
    
    /// <summary>
    /// Find the nearest player to the enemy
    /// </summary>
    /// <returns>A target player object instance</returns>
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
    
    /// <summary>
    /// Checks whether an enemy can see a target or not
    /// </summary>
    /// <returns>True if the enemy can see the target</returns>
    private bool CanSee(TargetPlayer target)
    {
        if (Physics.Linecast(transform.position + Vector3.up, target.Player.transform.position + Vector3.up, out RaycastHit hit))
        {
            return (target.Player.transform.position - transform.position).sqrMagnitude <= aggroRadius * aggroRadius && hit.transform.gameObject == target.Player;
        }
        return false;
    }
    
    /// <summary>
    /// Get the transform of the current target
    /// </summary>
    /// <returns>The transform of the current target (if any)</returns>
    public Transform GetCurrentTarget()
    {
        return _currentTarget?.Player.transform;
    }
    
    /// <summary>
    /// Update the aggro value of a certain target
    /// </summary>
    /// <param name="player">Player GameObject that is attacking the enemy</param>
    /// <param name="damageAmount">Amount to update the targets aggro value by</param>
    public void OnDamageTaken(GameObject player, float damageAmount)
    {
        // Update aggro value with amount of damage taken
        TargetPlayer target = _targets.Find(t => t.Player == player);
        
        target.Aggro += damageAmount;
        target.IsKnown = true;
    }
    
    /// <summary>
    /// Set the stun value of the enemy
    /// </summary>
    /// <param name="isStunned">Indictates whether the enemy is stunned or not</param>
    public void SetStunned(bool isStunned)
    {
        IsStunned = isStunned;
    }
    
    /// <summary>
    /// Draw the wire sphere of the enemy's aggro radius when selected in the scene
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Draw the aggro radius of the enemy in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
