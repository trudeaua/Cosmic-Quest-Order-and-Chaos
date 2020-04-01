using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamTask : Task
{
    struct TaskPart
    {
        public GameObject playerObject;
        public string className;
        public bool completed;
        public bool started;
        public DialogueTrigger dialogueTrigger;
    }

    public DialogueTrigger mageDialogue;
    public DialogueTrigger meleeDialogue;
    public DialogueTrigger healerDialogue;
    public DialogueTrigger rangedDialogue;

    private TaskPart[] taskParts;
    private int currentTask;

    protected override void SetupTask()
    {
        if (PlayersInTaskArea == numPlayers)
        {
            taskParts = new TaskPart[numPlayers];
            currentTask = 0;
            for (int i = 0; i < taskParts.Length; i++)
            {
                taskParts[i].playerObject = PlayerManager.Instance.FindPlayer(i);
                taskParts[i].className = PlayerManager.Instance.GetPlayerClassName(i);
                taskParts[i].completed = false;
                taskParts[i].started = false;
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
    }

    public override void StartTask()
    {
        for (int i = 0; i < taskParts.Length; i++)
        {
            PlayerMotorController playerMotor = taskParts[i].playerObject.GetComponent<PlayerMotorController>();
            PlayerStatsController playerStats = taskParts[i].playerObject.GetComponent<PlayerStatsController>();
            if (i == currentTask)
            {
                playerMotor.StopParalyze();
                CharacterColour colour = playerStats.characterColour;
                _Puzzle.SetPuzzleColour(colour);
            }
            else
            {
                playerMotor.StartParalyze();
            }
        }
        started = true;
        taskParts[currentTask].started = true;
        taskParts[currentTask].dialogueTrigger.TriggerDialogue();
    }

    public override void CompleteTask()
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
            completed = true;
            for (int i = 0; i < taskParts.Length; i++)
            {
                PlayerMotorController playerMotor = taskParts[i].playerObject.GetComponent<PlayerMotorController>();
                playerMotor.StopParalyze();
            }
        }
    }
}
