using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhakDeathBehaviour : StateMachineBehaviour
{
    public float deathDuration = 3f;
    
    private RhakStatsController _stats;
    private float _timer;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stats = animator.GetComponent<RhakStatsController>();
        
        // Trigger the death event
        _stats.RhakDeath();
        
        // Set the timer to trigger fade animation
        _timer = Mathf.Max(deathDuration, stateInfo.length);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            animator.gameObject.SetActive(false);
    }
}
