using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitIdleBehaviour : StateMachineBehaviour
{
    public float waitTime = 1f;
    public bool randomTime;
    public float minRandomTime;
    public float maxRandomTime;
    public string exitTrigger;
    
    private float _timer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = randomTime ? Random.Range(minRandomTime, maxRandomTime) : waitTime;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            animator.SetTrigger(exitTrigger);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
