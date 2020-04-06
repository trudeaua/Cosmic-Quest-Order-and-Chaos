using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class ActionPuzzle : Puzzle
{
    [Serializable]
    public class Action
    {
        public string actionName;
        public Image actionImage;
        public DialogueTrigger dialogueTrigger;
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

    public class ActionTriggeredEvent : UnityEvent<int>
    {
        public InputActionMap ActionMap;
        public int DeviceId;
        public string ActionName;
        public UnityEvent onActionTriggered;

        public ActionTriggeredEvent(int _DeviceId, string _ActionName, InputActionMap _ActionMap)
        {
            ActionMap = _ActionMap;
            DeviceId = _DeviceId;
            ActionName = _ActionName;
            onActionTriggered = new UnityEvent();
            ActionMap.actionTriggered += HandleAction;
        }

        public void HandleAction(InputAction.CallbackContext context)
        {
            if (context.action.name == ActionName && context.performed)
            {
                Invoke(DeviceId);
                //ActionMap.actionTriggered -= HandleAction;
            }
        }
    }
    public RectTransform overlay;
    public Animator[] playerCheckboxAnims;
    public Action[] requiredActions;
    private List<int> completedActionIds;
    private List<PlayerInput> playerInputs;
    private int currentAction;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DisableOverlay(0));
        completedActionIds = new List<int>();
        playerInputs = new List<PlayerInput>();
        currentAction = -1;
        for (int i = 0; i < requiredActions.Length; i++)
        {
            requiredActions[i].actionImage.enabled = false;
        }
    }

    /// <summary>
    /// Add a player input to this puzzle
    /// </summary>
    /// <param name="_playerInput">Player's input</param>
    public virtual void AddPlayerInput(PlayerInput _playerInput)
    {
        int _deviceId = _playerInput.devices.FirstOrDefault().deviceId;
        // check if the player input has been added already
        foreach (PlayerInput playerInput in playerInputs)
        {
            int deviceId = playerInput.devices.FirstOrDefault().deviceId;
            if (_deviceId == deviceId)
                return;
        }
        playerInputs.Add(_playerInput);
    }

    /// <summary>
    /// Setup the task
    /// </summary>
    protected virtual void Setup()
    {
        completedActionIds.Clear();
        for (int i = 0; i < playerInputs.Count; i++)
        {
            ReadOnlyArray<InputAction> inputActions = playerInputs[i].currentActionMap.actions;
            // Disable all required actions
            for (int j = 0; j < requiredActions.Length; j++)
            {
                InputAction action = playerInputs[i].currentActionMap.actions.First(e => e.name == requiredActions[j].actionName);
                action.Disable();
            }
        }
        
        requiredActions[currentAction+1].dialogueTrigger.TriggerDialogue();
    }

    /// <summary>
    /// Listen for events from the player action map
    /// </summary>
    public void ListenForActions()
    {
        EnableOverlay();
        requiredActions[currentAction].actionImage.enabled = true;
        // Listen for the currently required action being triggered on each player's input
        for (int i = 0; i < playerInputs.Count; i++)
        {
            int deviceId = playerInputs[i].devices.FirstOrDefault().deviceId;
            ActionTriggeredEvent actionListenerEvent = new ActionTriggeredEvent(deviceId, requiredActions[currentAction].actionName, playerInputs[i].currentActionMap);
            actionListenerEvent.AddListener(ActionCompleted);
        }
    }

    /// <summary>
    /// Stop listening for events from the player action map
    /// </summary>
    public void StopListeningForActions()
    {
        // Remove the listener for the currently required action
        for (int i = 0; i < playerInputs.Count; i++)
        {
            int deviceId = playerInputs[i].devices.FirstOrDefault().deviceId;
            ActionTriggeredEvent actionListenerEvent = new ActionTriggeredEvent(deviceId, requiredActions[currentAction].actionName, playerInputs[i].currentActionMap);
            actionListenerEvent.RemoveListener(ActionCompleted);
        }
    }

    public void ActionCompleted(int deviceId)
    {
        if (completedActionIds.Contains(deviceId))
            return;

        int playerNumber = PlayerManager.Instance.GetPlayerNumber(deviceId);
        if (playerNumber > -1)
        {
            playerCheckboxAnims[playerNumber].SetBool("isChecked", true);
            requiredActions[currentAction].SetComplete();
        }
        
        completedActionIds.Add(deviceId);

        // All players have completed the currently required action
        if (completedActionIds.Count == playerInputs.Count)
        {
            StopListeningForActions();
            SetComplete();
        }
    } 

    /// <summary>
    /// Disable the current target action for all players
    /// </summary>
    private void DisableCurrentAction()
    {   
        if (currentAction == -1)
            return;
        
        for (int i = 0; i < playerInputs.Count; i++)
        {
            InputAction inputAction = playerInputs[i].currentActionMap.actions.First(e => e.name == requiredActions[currentAction].actionName);
            inputAction.Disable();
        }
    }

    /// <summary>
    /// Enable all actions on each player's action map
    /// </summary>
    public void EnableAllActions()
    {
        foreach (PlayerInput playerInput in playerInputs)
        {
            foreach (InputAction inputAction in playerInput.currentActionMap.actions)
            {
                inputAction.Enable();
            }
        }
    }

    /// <summary>
    /// Enable the action that follows the current one
    /// </summary>
    public void EnableNextAction()
    {
        foreach (PlayerInput playerInput in playerInputs)
        {
            DisableCurrentAction();
            InputAction nextAction = playerInput.currentActionMap.actions.First(e => e.name == requiredActions[currentAction+1].actionName);
            nextAction.Enable();
        }
        currentAction += 1;
        completedActionIds.Clear();
    }

    /// <summary>
    /// Reset the puzzle
    /// </summary>
    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
        foreach (Action requiredAction in requiredActions)
        {
            requiredAction.SetIncomplete();
        }
        Setup();
    }

    protected override void SetComplete()
    {
        if (currentAction + 1 < requiredActions.Length)
            requiredActions[currentAction + 1].dialogueTrigger.TriggerDialogue();
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.GetComponent<PlayerCombatController>().SendMessage("ReleaseChargedAttack");
        }
        if (requiredActions.Count(e => e.IsCompleted()) == requiredActions.Length)
        {
            base.SetComplete(); 
            EnableAllActions();
        }
        else
        {
            Setup();
        }
        StartCoroutine(DisableOverlay(2));
    }

    /// <summary>
    /// Set the UI overlay to Active
    /// </summary>
    public void EnableOverlay()
    {
        overlay.gameObject.SetActive(true);
        requiredActions[currentAction].actionImage.enabled = true;
    }

    /// <summary>
    /// Set the UI overlay to Inactive
    /// </summary>
    public IEnumerator DisableOverlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        overlay.gameObject.SetActive(false);
        foreach (Animator animator in playerCheckboxAnims)
        {
            animator.SetBool("isChecked", false);
        }
        if (currentAction > -1)
            requiredActions[currentAction].actionImage.enabled = false;
    }
}
