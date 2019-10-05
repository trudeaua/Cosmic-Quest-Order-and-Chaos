using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float aggroRadius = 10f;
    public float deAggroRadius = 15f;

    Transform[] targets;
    Transform currentTarget;
    NavMeshAgent agent;

    void Start()
    {
        // Store references to the transforms of players
        targets = new Transform[PlayerManager.instance.players.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = PlayerManager.instance.players[i].transform;
        }
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // TODO For now just reference the first player in the targets array, will need to
        // switch to some kind of targeting algorithm

        float distance = Vector3.Distance(targets[0].position, transform.position);

        if (currentTarget == null && distance <= aggroRadius)
        {
            // We don't have a target and there may be one we can detect
            RaycastHit hit;
            Physics.Linecast(transform.position, targets[0].position, out hit);
            if (hit.transform.CompareTag("Player"))
            {
                // Can only aggro if the player is visible
                currentTarget = targets[0];
            }
        }
        else if (currentTarget != null)
        {
            if (distance <= deAggroRadius)
            {
                // We have a target so let's follow them
                agent.SetDestination(currentTarget.position);

                if (distance <= agent.stoppingDistance)
                {
                    // Attack target?
                    FaceTarget();
                }
            }
            else
            {
                // Target out of range, cancel enemy aggro
                currentTarget = null;
                agent.SetDestination(transform.position);
            }
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
