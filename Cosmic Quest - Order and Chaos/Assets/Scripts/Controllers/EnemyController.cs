using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyCombat))]
public class EnemyController : MonoBehaviour
{
    public float aggroRadius = 10f;
    public float deAggroRadius = 15f;

    private EnemyCombat combat;
    private EnemyStats stats;
    private Animator anim;

    private List<GameObject> targets;
    private Transform currentTarget;
    private NavMeshAgent agent;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<EnemyCombat>();
        stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        // Store references to the transforms of players
        targets = PlayerManager.instance.players;
    }

    private void FixedUpdate()
    {
        // Prevent enemy activity during death animation
        if (stats.isDead)
        {
            return;
        }

        // TODO Should enemies wander when no target is selected?

        // Enemy target selection follows this precedence:
        //   1. Player who attacked them last (TODO)
        //   2. Player they are currently following
        //   3. Closest player in their sight

        // No target selected, try to find one
        if (currentTarget == null)
        {
            float minDistance = float.MaxValue;

            // Find the nearest player within enemy's sight
            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance <= aggroRadius && distance < minDistance)
                {
                    // Ensure enemy can see them directly
                    if (Physics.Linecast(transform.position, target.transform.position, out RaycastHit hit) && hit.transform.CompareTag("Player"))
                    {
                        currentTarget = target.transform;
                        minDistance = distance;
                    }
                }
            }
        }
        
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(currentTarget.position, transform.position);
            if (distance <= deAggroRadius)
            {
                agent.SetDestination(currentTarget.position);

                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();

                    // TODO probably not the most efficient way to handle enemy combat decisions?
                    combat.PrimaryAttack();

                    // Check to see if current target died -- TODO move this elsewhere?
                    if (currentTarget.GetComponent<PlayerStats>().isDead)
                    {
                        currentTarget = null;
                    }
                }
            }
            else
            {
                // Target out of range, cancel enemy aggro
                // Enemy will still move to the last known location of the player
                currentTarget = null;
            }
        }

        // Set walk animation
        if (agent.velocity != Vector3.zero)
        {
            anim.SetBool("Walk Forward", true);
        }
        else
        {
            anim.SetBool("Walk Forward", false);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the aggro and de-aggro radii of the enemy in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, deAggroRadius);
    }
}
