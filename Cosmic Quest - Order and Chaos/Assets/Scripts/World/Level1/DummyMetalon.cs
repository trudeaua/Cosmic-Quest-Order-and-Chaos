using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyMetalon : MonoBehaviour
{
    private NavMeshAgent _agent;
    public GameObject Destination;   // Set exit portal

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {   
        // Set dummy metalons to infintely walk to to destination of entrance portal
        _agent.SetDestination(Destination.transform.position);
    }
}
