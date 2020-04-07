using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

/// <summary>
/// Variant of a puzzle that prompts players to perform actions on their controllers
/// </summary>
public class ActionPuzzle : Puzzle
{
    /// <summary>
    /// Action required for the action puzzle
    /// </summary>
    [Serializable]
    protected class RequiredAction
    {
        [Tooltip("Name of the action")]
        public string actionName;

        [Tooltip("Action icon image (e.g. PS4 square button icon)")]
        public Image actionImage;

        [Tooltip("Dialogue to play before performing class specific dialogue")]
        public DialogueTrigger introDialogueTrigger;
        [Tooltip("Dialogue to play after performing class specific dialogue")]
        public DialogueTrigger exitDialogueTrigger;

        [Tooltip("Specific dialogue to play for mage's action")]
        public DialogueTrigger mageDialogueTrigger;
        [Tooltip("Specific dialogue to play for mage's action")]
        public DialogueTrigger meleeDialogueTrigger;
        [Tooltip("Specific dialogue to play for mage's action")]
        public DialogueTrigger healerDialogueTrigger;
        [Tooltip("Specific dialogue to play for mage's action")]
        public DialogueTrigger rangedDialogueTrigger;

        /// <summary>
        /// Whether the action has been completed or not
        /// </summary>
        private bool completed;

        /// <summary>
        /// Marks the action as completed
        /// </summary>
        public void SetComplete()
        {
            completed = true;
        }

        /// <summary>
        /// Marks the action as incomplete
        /// </summary>
        public void SetIncomplete()
        {
            completed = false;
        }

        /// <summary>
        /// Checks if the action is completd
        /// </summary>
        /// <returns>True if the action is completed</returns>
        public bool IsCompleted()
        {
            return completed;
        }
    }

    /// <summary>
    /// Variant of a Unity Event that emits the id of the device that triggered an action
    /// Rationale: System.Action class doesn't allow you to pass parameters which was needed for this functionality
    /// </summary>
    protected class ActionTriggeredEvent : UnityEvent<int>
    {
        /// <summary>
        /// Action map that will be listened to for events
        /// </summary>
        private InputActionMap actionMap;

        /// <summary>
        /// Id of the device we're listening to
        /// </summary>
        private int deviceId;

        /// <summary>
        /// Name of the action to listen for
        /// </summary>
        private string actionName;

        /// <summary>
        /// Constructor for ActionTriggeredEvent
        /// </summary>
        /// <param name="_DeviceId">Id of a device being listening to</param>
        /// <param name="_ActionName">Name of the action to listen for</param>
        /// <param name="_ActionMap">Action map that will be listened to for events</param>
        public ActionTriggeredEvent(int _deviceId, string _actionName, InputActionMap _actionMap)
        {
            actionMap = _actionMap;
            deviceId = _deviceId;
            actionName = _actionName;
            actionMap.actionTriggered += HandleAction;
        }

        /// <summary>
        /// Callback method for handling input actions
        /// </summary>
        /// <param name="actionContext"></param>
        private void HandleAction(InputAction.CallbackContext actionContext)
        {
            if (actionContext.action.name == actionName && actionContext.performed)
            {
                Invoke(deviceId);
            }
        }
    }

    [Tooltip("UI overlay for displaying an action to complete")]
    [SerializeField] private RectTransform actionOverlay;
    [Tooltip("Animator controllers for each player's action-completed checkmark")]
    [SerializeField] private Animator[] playerCheckmarkAnims;
    [Tooltip("Actions required to be performed in order for the puzzle to complete")]
    [SerializeField] private RequiredAction[] requiredActions;

    /// <summary>
    /// List of device ids that have completed the current action
    /// </summary>
    private List<int> completedActionIds;

    /// <summary>
    /// List of player inputs to listen to
    /// </summary>
    private List<PlayerInput> playerInputs;

    /// <summary>
    /// List of player classes
    /// Rationale: For displaying class specific dialogue
    /// </summary>
    private List<string> playerClasses;

    /// <summary>
    /// Maintains which action is currently being listened for
    /// </summary>
    private int currentAction;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DisableOverlay(0));
        completedActionIds = new List<int>();
        playerInputs = new List<PlayerInput>();
        playerClasses = new List<string>();
        currentAction = -1;

        // hide all action icons
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
        int playerNumber = PlayerManager.Instance.GetPlayerNumber(_deviceId);
        string className = PlayerManager.Instance.GetPlayerClassName(playerNumber);
        if (!playerClasses.Contains(className))
            playerClasses.Add(className);
    }

    public void PlayClassSpecificDialogue()
    {
        if (playerClasses.Count > 1)
        {
            DialogueTrigger curr = GetClassSpecificDialogue(playerClasses[0]);
            DialogueTrigger next = null;
            curr.TriggerDialogue();
            for (int i = 1; i < playerClasses.Count; i++)
            {
                next = GetClassSpecificDialogue(playerClasses[i]);
                curr.dialogue.onComplete.AddListener(next.TriggerDialogue);
                curr = next;
            }
            next.dialogue.onComplete.AddListener(requiredActions[currentAction+1].exitDialogueTrigger.TriggerDialogue);
        }
        else if (playerClasses.Count == 1)
        {
            DialogueTrigger curr = GetClassSpecificDialogue(playerClasses[0]);
            curr.dialogue.onComplete.AddListener(requiredActions[currentAction + 1].exitDialogueTrigger.TriggerDialogue);
            curr.TriggerDialogue();
        }
        else
        {
            requiredActions[currentAction+1].exitDialogueTrigger.TriggerDialogue();
        }
    }

    /// <summary>
    /// Get the dialogue trigger associated with a class name
    /// </summary>
    /// <param name="className">Name of a player class</param>
    /// <returns>The dialogue trigger for a specific player class, null if not found</returns>
    private DialogueTrigger GetClassSpecificDialogue(string className)
    {
        switch (className)
        {
            case "Mage":
                return requiredActions[currentAction+1].mageDialogueTrigger;
            case "Melee":
                return requiredActions[currentAction+1].meleeDialogueTrigger;
            case "Healer":
                return requiredActions[currentAction+1].healerDialogueTrigger;
            case "Ranged":
                return requiredActions[currentAction+1].rangedDialogueTrigger;
            default:
                return null;
        }
    }

    /// <summary>
    /// Set up the puzzle
    /// </summary>
    protected void Setup()
    {
        // empty the list of device ids that have completed the actions
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
        // trigger the introductory dialogue
        requiredActions[currentAction + 1].introDialogueTrigger.TriggerDialogue();
    }

    /// <summary>
    /// Listen for events from the player action map
    /// </summary>
    public void ListenForActions()
    {
        // hide player checkmarks
        foreach (Animator animator in playerCheckmarkAnims)
        {
            animator.SetBool("isChecked", false);
            animator.gameObject.SetActive(false);
        }
        EnableOverlay();
        requiredActions[currentAction].actionImage.enabled = true;
        // Listen for the currently required action being triggered on each player's input
        for (int i = 0; i < playerInputs.Count; i++)
        {
            int deviceId = playerInputs[i].devices.FirstOrDefault().deviceId;
            ActionTriggeredEvent actionListenerEvent = new ActionTriggeredEvent(deviceId, requiredActions[currentAction].actionName, playerInputs[i].currentActionMap);
            actionListenerEvent.AddListener(CompleteAction);
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
            actionListenerEvent.RemoveListener(CompleteAction);
        }
    }

    /// <summary>
    /// Registers that a device with `deviceId` has completed the currently required action
    /// </summary>
    /// <param name="deviceId">Id of the device that completed the action</param>
    private void CompleteAction(int deviceId)
    {
        if (completedActionIds.Contains(deviceId))
            return;

        int playerNumber = PlayerManager.Instance.GetPlayerNumber(deviceId);
        if (playerNumber > -1)
        {
            playerCheckmarkAnims[playerNumber].gameObject.SetActive(true);
            playerCheckmarkAnims[playerNumber].SetBool("isChecked", true);
            requiredActions[currentAction].SetComplete();
        }
        
        completedActionIds.Add(deviceId);

        // Complete if all players have performed the currently required action
        if (completedActionIds.Count == playerInputs.Count)
        {
            StopListeningForActions();
            SetComplete();
        }
    } 

    /// <summary>
    /// Disable the currently required action for all players
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
        foreach (RequiredAction requiredAction in requiredActions)
        {
            requiredAction.SetIncomplete();
        }
        Setup();
    }

    /// <summary>
    /// Cancels any charged attacks on the players' combat controllers
    /// Rationale: When the puzzle disables an action and the player is performing a charged attack, they will be stuck. This fixes that.
    /// </summary>
    /// <param name="delay">Amount of time to wait before cancelling the charged attacks</param>
    /// <returns></returns>
    protected IEnumerator ReleaseChargedAttacks(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.GetComponent<PlayerCombatController>().SendMessage("ReleaseChargedAttack");
        }
    }

    /// <summary>
    /// Attempt to complete the puzzle
    /// </summary>
    protected override void SetComplete()
    {

        if (currentAction + 1 < requiredActions.Length)
            requiredActions[currentAction + 1].introDialogueTrigger.TriggerDialogue();
        StartCoroutine(ReleaseChargedAttacks(2));
        // Only complete if all required actions are completed, else set up the puzzle again
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
        actionOverlay.gameObject.SetActive(true);
        requiredActions[currentAction].actionImage.enabled = true;
    }

    /// <summary>
    /// Set the UI overlay to Inactive
    /// </summary>
    public IEnumerator DisableOverlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        actionOverlay.gameObject.SetActive(false);
        if (currentAction > -1)
            requiredActions[currentAction].actionImage.enabled = false;
    }
}
