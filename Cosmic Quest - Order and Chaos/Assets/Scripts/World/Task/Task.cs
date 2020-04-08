using System.Linq;
using UnityEngine;

/// <summary>
/// Class that encapsulates dialogue and puzzles
/// </summary>
public class Task : MonoBehaviour
{
    [Tooltip("Dialogue to play after entering the task area")]
    public DialogueTrigger introDialogueTrigger;
    [Tooltip("Dialogue to play after leaving the task area")]
    public DialogueTrigger exitDialogueTrigger;

    /// <summary>
    /// Doors in the task area
    /// </summary>
    protected Door[] doors;

    /// <summary>
    /// Indicates whether the task completed or not
    /// </summary>
    protected bool completed;

    /// <summary>
    /// Indicates whether the task has been started or not
    /// </summary>
    protected bool started;

    /// <summary>
    /// Number of players in the task area
    /// </summary>
    protected int playersInTaskArea = 0;

    /// <summary>
    /// Number of players playing the game
    /// </summary>
    protected int numPlayers;

    /// <summary>
    /// Puzzles in the task area
    /// </summary>
    protected Puzzle[] puzzles;

    protected virtual void Start()
    {
        numPlayers = PlayerManager.Instance.NumPlayers;
        doors = GetComponentsInChildren<Door>();
        puzzles = GetComponents<Puzzle>();
        // Open doors by default
        OpenDoors();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTaskArea += 1;
            // once all players are in the task area, the task begins
            if (playersInTaskArea == numPlayers && started == false)
            {
                CloseDoors();
                SetupTask();
                PlayIntroDialogue();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTaskArea -= 1;
        }
    }

    /// <summary>
    /// Play any introductory task dialogue
    /// </summary>
    protected virtual void PlayIntroDialogue()
    {
        if (introDialogueTrigger != null)
        {
            introDialogueTrigger.TriggerDialogue();
        }
    }

    /// <summary>
    /// Play any exiting task dialogue
    /// </summary>
    protected virtual void PlayExitDialogue()
    {
        if (exitDialogueTrigger != null)
        {
            exitDialogueTrigger.TriggerDialogue();
        }
    }

    /// <summary>
    /// Open any doors associated with the task
    /// </summary>
    public virtual void OpenDoors()
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
    }

    /// <summary>
    /// Open any doors associated with the task
    /// </summary>
    public virtual void CloseDoors()
    {
        foreach (Door door in doors)
        {
            door.Close();
        }
    }

    /// <summary>
    /// Setup the task
    /// </summary>
    protected virtual void SetupTask()
    {
        started = false;
        completed = false;
        // if there's no dialogue then we just start the task
        if (introDialogueTrigger == null)
            foreach (Puzzle puzzle in puzzles)
            {
                puzzle.ResetPuzzle();
            }
        StartTask();
    }

    /// <summary>
    /// Start the task
    /// </summary>
    public virtual void StartTask()
    {
        started = true;
    }

    /// <summary>
    /// Complete the task
    /// </summary>
    public virtual void Complete()
    {
        if (puzzles.Count(e => e.isComplete) > 0)
        {
            completed = true;
            PlayExitDialogue();
            OpenDoors();
        }
    }
}
