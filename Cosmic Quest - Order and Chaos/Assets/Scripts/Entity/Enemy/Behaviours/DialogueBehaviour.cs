using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehaviour : StateMachineBehaviour
{
    public DialogueTrigger dialogueTrigger;
    public float startDelay;
    public float endDelay;
    public string exitTrigger;
    private float _timer;
    private bool _inStartDelay;
    private bool _inEndDelay;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = startDelay;
        _inStartDelay = true;
        _inEndDelay = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
            return;

        // If currently in a delay period
        if (_inStartDelay || _inEndDelay)
        {
            _timer -= Time.deltaTime;
            if (_timer > 0f)
                return;
            
            if (_inStartDelay)
            {
                // Start dialogue
                dialogueTrigger.TriggerDialogue();
                _inStartDelay = false;
                return;
            }

            if (_inEndDelay)
            {
                // Transition to next state
                animator.SetTrigger(exitTrigger);
                return;
            }
        }

        // Dialogue completed
        if (!DialogueManager.Instance.PlayingDialogue)
        {
            _timer = endDelay;
            _inEndDelay = true;
        }
    }
}
