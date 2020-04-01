using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    protected bool completed;
    protected bool started;
    protected int PlayersInTaskArea = 0;
    protected int numPlayers;
    protected Puzzle _Puzzle;
    protected virtual void Start()
    {
        numPlayers = PlayerManager.Instance.NumPlayers;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayersInTaskArea += 1;
            if (!started)
            {
                Puzzle puzzle = GetComponent<Puzzle>();
                if (puzzle != null)
                {
                    _Puzzle = puzzle;
                }
                SetupTask();
                PlayDialogue();
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

    protected virtual void PlayDialogue()
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.dialogue.onComplete.AddListener(StartTask);
            dialogueTrigger.TriggerDialogue();
        }
    }

    protected virtual void SetupTask()
    {
        started = false;
        completed = false;
        StartTask();
    }

    public virtual void StartTask()
    {
        started = true;
    }

    public virtual void CompleteTask()
    {
        completed = true;
        StartTask();
    }
}
