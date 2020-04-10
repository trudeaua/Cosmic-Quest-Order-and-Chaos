using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour
{
    public float deathDuration = 3f;
    public float fadeDuration = 0.5f;
    
    private float _timer;
    private SkinnedMeshRenderer _renderer;
    private float _fadeAmount;
    private float _fadeStep;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _renderer = animator.GetComponentInChildren<SkinnedMeshRenderer>();

        _fadeAmount = 0f;
        _fadeStep = 1f / fadeDuration;

        // Set the timer to trigger fade animation
        _timer = Mathf.Max(deathDuration, stateInfo.length) + fadeDuration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0f)
        {
            animator.gameObject.SetActive(false);
        }
        else if (_timer <= fadeDuration)
        {
            _fadeAmount += _fadeStep * Time.deltaTime;

            if (_fadeAmount > 1f)
                _fadeAmount = 1f;
            
            // Animate the dissolve amount
            _renderer.materials[0].SetFloat("_DissolveAmount", _fadeAmount);
        }
    }
}
