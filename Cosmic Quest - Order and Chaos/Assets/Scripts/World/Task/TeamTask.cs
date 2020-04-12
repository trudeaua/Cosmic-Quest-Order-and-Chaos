using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Task that requires that all players participate
/// </summary>
public class TeamTask : Task
{
    /// <summary>
    /// Maintains the players participating in the task
    /// </summary>
    private GameObject[] taskParticipants;

    /// <summary>
    /// Setup the task for all players
    /// </summary>
    protected override void SetupTask()
    {
        // if there's no dialogue then we just start the task
        if (introDialogueTrigger == null)
            foreach (Puzzle puzzle in puzzles)
            {
                puzzle.ResetPuzzle();
            }
        taskParticipants = new GameObject[numPlayers];
        for (int i = 0; i < taskParticipants.Length; i++)
        {
            taskParticipants[i] = PlayerManager.Instance.FindPlayer(i);

            // Listen for when a player dies
            PlayerStatsController playerStats = taskParticipants[i].GetComponent<PlayerStatsController>();
            playerStats.onDeath.AddListener(PlayerDied);
        }
    }

    /// <summary>
    /// Start a task for a single player, immobilize the other players while the player completes their task
    /// </summary>
    public override void StartTask()
    {
        for (int i = 0; i < taskParticipants.Length; i++)
        {
            PlayerInput playerInput = taskParticipants[i].GetComponent<PlayerInput>();
            foreach (Puzzle puzzle in puzzles)
            {
                // Behaviour for different puzzles goes here
                ActionPuzzle actionPuzzle = null;
                if (puzzle is ActionPuzzle && !puzzle.IsComplete)
                {
                    actionPuzzle = puzzle as ActionPuzzle;
                    actionPuzzle.AddPlayerInput(playerInput);
                }
            }
        }
        started = true;
    }

    /// <summary>
    /// Completes the task if all parts are completed. Starts the next task otherwise.
    /// </summary>
    public override void Complete()
    {
        // keep going if all puzzles in the task aren't completed
        if (puzzles.Count(e => e.IsComplete == false) > 0)
        {
            StartTask();
        }
        else
        {
            foreach (GameObject taskParticipant in taskParticipants)
            {
                PlayerStatsController playerStats = taskParticipant.GetComponent<PlayerStatsController>();
                playerStats.onDeath.RemoveListener(PlayerDied);
            }
            base.Complete();
        }
    }

    /// <summary>
    /// Callback for when a player in the task dies
    /// </summary>
    private void PlayerDied()
    {
        LevelManager.Instance.RestartCurrentLevel();
    }
}
