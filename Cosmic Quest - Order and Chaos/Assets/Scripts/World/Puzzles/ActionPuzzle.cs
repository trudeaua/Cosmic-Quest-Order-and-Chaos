using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ActionPuzzle : Puzzle
{
    [Serializable]
    public class Action
    {
        public string actionName;
        private bool completed;

        public void SetComplete()
        {
            completed = true;
        }

        public void SetIncomplete()
        {
            completed = false;
        }

        public bool IsCompleted()
        {
            return completed;
        }
    }
    public Action[] requiredActions;
    private PlayerInput playerInput;
    private InputActionMap playerActionMap;
    private int currentAction;
    private DialogueTrigger dialogueTrigger;

    /// <summary>
    /// Set the player input for this puzzle
    /// </summary>
    /// <param name="_playerInput">Player's input</param>
    public virtual void SetPlayerInput(PlayerInput _playerInput)
    {
        playerInput = _playerInput;
    }

    /// <summary>
    /// Setup the task
    /// </summary>
    protected virtual void Setup()
    {
        currentAction = -1;
        playerActionMap = PlayerManager.Instance.GetActionMap(playerInput, "Player");
        ReadOnlyArray<InputAction> inputActions = playerActionMap.actions;

        dialogueTrigger.TriggerDialogue();
        
        // Disable all required actions
        for (int i = 0; i < requiredActions.Length; i++)
        {
            InputAction action = playerActionMap.actions.First(e => e.name == requiredActions[i].actionName);
            action.Disable();
        }

        ListenForActions();
    }

    /// <summary>
    /// Listen for events from the player action map
    /// </summary>
    public void ListenForActions()
    {
        playerActionMap.actionTriggered += PlayerActionMap_actionTriggered;
    }

    /// <summary>
    /// Stop listening for events from the player action map
    /// </summary>
    public void StopListeningForActions()
    {
        playerActionMap.actionTriggered -= PlayerActionMap_actionTriggered;
    }

    /// <summary>
    /// Handle events from the player action map
    /// </summary>
    /// <param name="context"></param>
    private void PlayerActionMap_actionTriggered(InputAction.CallbackContext context)
    {
        for (int i = 0; i < requiredActions.Length; i++)
        {
            if (context.action.name == requiredActions[i].actionName)
            {
                if (context.performed && requiredActions[i].IsCompleted() == false)
                {
                    requiredActions[i].SetComplete();
                    if (requiredActions.Count(e => e.IsCompleted()) == requiredActions.Length)
                    {
                        StopListeningForActions();
                        EnableAllActions();
                        SetComplete();
                    }
                    else
                    {
                        dialogueTrigger.TriggerDialogue();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set the dialogue trigger that is played upon completing an action
    /// </summary>
    /// <param name="_dialogueTrigger"></param>
    public void SetDialogueTrigger(DialogueTrigger _dialogueTrigger)
    {
        dialogueTrigger = _dialogueTrigger;
    }

    /// <summary>
    /// Disable the current target action
    /// </summary>
    private void DisableCurrentAction()
    {
        if (currentAction > -1)
        {
            InputAction inputAction = playerActionMap.actions.First(e => e.name == requiredActions[currentAction].actionName);
            playerInput.GetComponent<PlayerCombatController>().SendMessage("ReleaseChargedAttack");
            inputAction.Disable();
        }
    }

    /// <summary>
    /// Enable all actions on the player action map
    /// </summary>
    public void EnableAllActions()
    {
        foreach (InputAction inputAction in playerActionMap.actions)
        {
            inputAction.Enable();
        }
    }

    /// <summary>
    /// Enable the action that follows the current one
    /// </summary>
    public void EnableNextAction()
    {
        DisableCurrentAction();
        InputAction nextAction = playerActionMap.actions.First(e => e.name == requiredActions[currentAction + 1].actionName);
        nextAction.Enable();
        currentAction += 1;
    }

    /// <summary>
    /// Reset the puzzle
    /// </summary>
    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
        currentAction = -1;
        foreach (Action requiredAction in requiredActions)
        {
            requiredAction.SetIncomplete();
        }
        Setup();
    }
}
