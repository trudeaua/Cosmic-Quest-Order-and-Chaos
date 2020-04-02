using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamTask : Task
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
                playerInput.ActivateInput();
                MoveToActiveArea(playerInput.gameObject);
                CharacterColour colour = playerStats.characterColour;
                foreach (Puzzle puzzle in _Puzzles)
                {
                    if (puzzle is EnemyPuzzle)
                    {
                        puzzle.SetPuzzleColour(colour);
                    }
                    else if (puzzle is ActionPuzzle)
                    {
                        ActionPuzzle actionPuzzle = puzzle as ActionPuzzle;
                        actionPuzzle.SetPlayerInput(playerInput);
                    }
                }
                Dialogue currentDialogue = taskParts[currentTask].dialogueTrigger.dialogue;
                for (int j = 0; j < currentDialogue.sentences.Length; j++)
                {
                    string color = PlayerManager.colours.GetColorHex(playerStats.characterColour);
                    string newText = currentDialogue.sentences[j].Replace("{PLAYER_NUMBER}", "<color=" + color + ">" + "Player " + (taskParts[i].playerNumber + 1) + "</color>");
                    currentDialogue.sentences[j] = newText;
                }
            }
            else
            {
                playerInput.PassivateInput();
                MoveToWaitingArea(playerInput.gameObject);
            }
        }
        started = true;
        taskParts[currentTask].started = true;
        taskParts[currentTask].dialogueTrigger.TriggerDialogue();
    }

    private void MoveToWaitingArea(GameObject playerObj)
    {
        playerObj.transform.position = WaitingArea.transform.position;
    }

    private void MoveToActiveArea(GameObject playerObj)
    {
        playerObj.transform.position = ActiveArea.transform.position;
    }

    private void EnableAllPlayerInput()
    {
        for (int i = 0; i < taskParts.Length; i++)
        {
            PlayerInput playerInput = taskParts[i].playerObject.GetComponent<PlayerInput>();
            playerInput.ActivateInput();
        }
    }
    /// <summary>
    /// Complete the task
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
            EnableAllPlayerInput();
            base.Complete();
        }
    }
}
