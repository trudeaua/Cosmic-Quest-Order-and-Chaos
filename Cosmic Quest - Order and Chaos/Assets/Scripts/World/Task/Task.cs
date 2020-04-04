using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public DialogueTrigger introDialogueTrigger;
    public DialogueTrigger exitDialogueTrigger;
    protected Door[] doors;
    protected bool completed;
    protected bool started;
    protected int PlayersInTaskArea = 0;
    protected int numPlayers;
    protected Puzzle[] _Puzzles;

    protected virtual void Start()
    {
        numPlayers = PlayerManager.Instance.NumPlayers;
        doors = GetComponentsInChildren<Door>();
        OpenDoors();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayersInTaskArea += 1;
            if (PlayersInTaskArea == numPlayers && !started)
            {
                CloseDoors();
                Puzzle[] puzzles = GetComponents<Puzzle>();
                if (puzzles != null)
                {
                    _Puzzles = puzzles;
                }
                SetupTask();
                PlayIntroDialogue();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayersInTaskArea -= 1;
        }
    }

    /// <summary>
    /// Play any introductory task dialogue
    /// </summary>
    protected virtual void PlayIntroDialogue()
    {
        if (introDialogueTrigger != null)
        {
            introDialogueTrigger.dialogue.onComplete.AddListener(StartTask);
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
        completed = true;
        PlayExitDialogue();
        OpenDoors();
    }
}
