using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyMetalon : MonoBehaviour
{
    public NavMeshAgent _agent;
    public Transform portalEnter;
    public Vector3 _position;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _position = transform.parent.Find("Portal_exit").position;
        portalEnter = transform.parent.Find("Portal_enter"); 

    }
    
    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(portalEnter.position);
    }
}
