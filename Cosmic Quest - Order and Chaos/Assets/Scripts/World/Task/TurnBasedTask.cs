using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TurnBasedTask : Task
{
    struct TaskPart
    {
        public GameObject playerObject;
        public string className;
        public bool completed;
        public bool started;
        public DialogueTrigger dialogueTrigger;
        public int playerNumber;
    }

    public DialogueTrigger mageDialogue;
    public DialogueTrigger meleeDialogue;
    public DialogueTrigger healerDialogue;
    public DialogueTrigger rangedDialogue;
    public Collider WaitingArea;
    public Collider ActiveArea;
    private TaskPart[] taskParts;
    private int currentTask;

    /// <summary>
    /// Setup the task for all players
    /// </summary>
    protected override void SetupTask()
    {
        taskParts = new TaskPart[numPlayers];
        currentTask = 0;
        for (int i = 0; i < taskParts.Length; i++)
        {
            int playerNumber = i;
            taskParts[i].playerObject = PlayerManager.Instance.FindPlayer(playerNumber);
            taskParts[i].className = PlayerManager.Instance.GetPlayerClassName(playerNumber);
            taskParts[i].completed = false;
            taskParts[i].started = false;
            taskParts[i].playerNumber = playerNumber;

            // Restart level if player dies during task
            PlayerStatsController playerStats = taskParts[i].playerObject.GetComponent<PlayerStatsController>();
            playerStats.onDeath.AddListener(PlayerDied);

            switch (taskParts[i].className)
            {
                case "Mage":
                    taskParts[i].dialogueTrigger = mageDialogue;
                    break;
                case "Melee":
                    taskParts[i].dialogueTrigger = meleeDialogue;
                    break;
                case "Healer":
                    taskParts[i].dialogueTrigger = healerDialogue;
                    break;
                case "Ranged":
                    taskParts[i].dialogueTrigger = rangedDialogue;
                    break;
                default:
                    continue;
            }
        }
    }

    /// <summary>
    /// Start a task for a single player, immobilize the other players while the player completes their task
    /// </summary>
    public override void StartTask()
    {
        for (int i = 0; i < taskParts.Length; i++)
        {
            PlayerStatsController playerStats = taskParts[i].playerObject.GetComponent<PlayerStatsController>();
            PlayerInput playerInput = taskParts[i].playerObject.GetComponent<PlayerInput>();
            if (taskParts[i].playerNumber == currentTask)
            {
                MoveToActiveArea(playerInput.gameObject, i);
                foreach (Puzzle puzzle in _Puzzles)
                {
                    if (puzzle is ActionPuzzle)
                    {
                        ActionPuzzle actionPuzzle = puzzle as ActionPuzzle;
                        actionPuzzle.SetPlayerInput(playerInput);
                    }
                    if (puzzle is EnemyPuzzle)
                    {
                        EnemyPuzzle enemyPuzzle = puzzle as EnemyPuzzle;
                        enemyPuzzle.SetPuzzleColour(playerStats.characterColour);
                    }
                }
                Dialogue currentDialogue = taskParts[currentTask].dialogueTrigger.dialogue;
                for (int j = 0; j < currentDialogue.sentences.Length; j++)
                {
                    string color = PlayerManager.colours.GetColorHex(playerStats.characterColour);
                    string newText = string.Copy(currentDialogue.sentences[j]).Replace("{PLAYER_NUMBER}", "<color=" + color + ">" + "Player " + (taskParts[i].playerNumber + 1) + "</color>");
                    currentDialogue.sentences[j] = newText;
                }
            }
            else
            {
                MoveToWaitingArea(playerInput.gameObject, i);
            }
        }
        started = true;
        taskParts[currentTask].started = true;
        taskParts[currentTask].dialogueTrigger.TriggerDialogue();
    }

    private void MoveToWaitingArea(GameObject playerObj, int playerNumber)
    {
        Vector3 from = playerObj.transform.position;
        Vector3 to = WaitingArea.transform.position;
        PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();
        StartCoroutine(MovePlayer(playerInput, to, playerNumber, false));
    }

    private void MoveToActiveArea(GameObject playerObj, int playerNumber)
    {
        Vector3 from = playerObj.transform.position;
        Vector3 to = ActiveArea.transform.position;
        PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();
        StartCoroutine(MovePlayer(playerInput, to, playerNumber));
    }

    /// <summary>
    /// Mocks `playerInput` and moves the player from their position to `to`
    /// </summary>
    /// <param name="to">Position to move the player to</param>
    /// <param name="playerInput">PlayerInput component attached to a player object</param>
    /// <param name="playerNumber">Number of the player</param>
    /// <param name="enabledUponRelease">Whether to enable the players original input upon being released from the mocked player input</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator MovePlayer(PlayerInput playerInput, Vector3 to, int playerNumber, bool enabledUponRelease = true)
    {
        Vector3 direction = to - playerInput.transform.position;
        float oldDist = Mathf.Infinity;
        float currDist = Vector3.Distance(playerInput.transform.position, to);

        // Setup the mock gamepad
        ReadOnlyArray<InputDevice>? inputDevices = playerInput.currentActionMap.devices;
        int[] deviceIds = inputDevices.Value.Select(e => e.deviceId).ToArray();
        PlayerInputMock playerInputMock = new PlayerInputMock(playerNumber, deviceIds, playerInput.currentControlScheme);
        playerInputMock.SetInputToMockGamepad(playerInput);

        // Start moving them towards the target position
        playerInputMock.Press(playerInputMock.Gamepad.leftStick, new Vector2(direction.x, direction.z));
        playerInputMock.Press(playerInputMock.Gamepad.rightStick, new Vector2(direction.x, direction.z));
        float timeout = 8;
        float tolerance = 0.05f;
        while (oldDist > currDist && timeout > 0)
        {
            yield return new WaitForSeconds(tolerance);
            timeout -= tolerance;
            oldDist = currDist;
            currDist = Vector3.Distance(playerInput.transform.position, to);
        }
        playerInputMock.Release(playerInputMock.Gamepad.leftStick, Vector2.zero);
        playerInputMock.Press(playerInputMock.Gamepad.rightStick, Vector2.up);
        yield return new WaitForSeconds(0.3f);
        playerInputMock.Release(playerInputMock.Gamepad.rightStick, Vector2.zero);

        // Restore their input device to what it was originally
        playerInputMock.ResetPlayerInput(playerInput);
        if (enabledUponRelease)
        {
            playerInput.ActivateInput();
        }
        else
        {
            playerInput.PassivateInput();
        }
    }

    private void PlayerDied()
    {
        // TODO what should be done here?
        Complete();
    }

    /// <summary>
    /// Enable the player input component of all players in the task
    /// </summary>
    private void EnableAllPlayerInput()
    {
        for (int i = 0; i < taskParts.Length; i++)
        {
            PlayerInput playerInput = taskParts[i].playerObject.GetComponent<PlayerInput>();
            playerInput.ActivateInput();
        }
    }
    /// <summary>
    /// Completes the task if all parts are completed. Starts the next task otherwise.
    /// </summary>
    public override void Complete()
    {
        taskParts[currentTask].completed = true;
        currentTask += 1;

        // keep going if all tasks aren't completed
        if (taskParts.Count(e => e.completed == false) > 0)
        {
            StartTask();
        }
        else
        {
            foreach (TaskPart taskPart in taskParts)
            {
                PlayerStatsController playerStats = taskPart.playerObject.GetComponent<PlayerStatsController>();
                playerStats.onDeath.RemoveListener(PlayerDied);
            }
            EnableAllPlayerInput();
            base.Complete();
        }
    }
}
