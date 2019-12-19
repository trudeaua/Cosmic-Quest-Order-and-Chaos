using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyMetalon : MonoBehaviour
{
    public Animator Anim;
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();   
        _agent = GetComponent<NavMeshAgent>();
        Anim.SetFloat("WalkSpeed", 5f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
