using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    struct TutorialPart
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

    private int numPlayers;
    private TutorialPart[] tutorialParts;
    private int currentTutorial;

    public void SetupTutorialParts()
    {
        numPlayers = PlayerManager.Instance.NumPlayers;
        tutorialParts = new TutorialPart[numPlayers];
        currentTutorial = 0;
        for (int i = 0; i < numPlayers; i++)
        {
            tutorialParts[i].playerObject = PlayerManager.Instance.FindPlayer(i);
            tutorialParts[i].className = PlayerManager.Instance.GetPlayerClassName(i);
            tutorialParts[i].completed = false;
            tutorialParts[i].started = false;
            switch (tutorialParts[i].className) {
                case "Mage":
                    tutorialParts[i].dialogueTrigger = mageDialogue;
                    break;
                case "Melee":
                    tutorialParts[i].dialogueTrigger = meleeDialogue;
                    break;
                case "Healer":
                    tutorialParts[i].dialogueTrigger = healerDialogue;
                    break;
                case "Ranged":
                    tutorialParts[i].dialogueTrigger = rangedDialogue;
                    break;
                default:
                    continue;
            }
        }
    }

    public void StartTutorialPart()
    {
        tutorialParts[currentTutorial].dialogueTrigger.TriggerDialogue();
        tutorialParts[currentTutorial].started = true;
    }

    public void CompleteTutorialPart()
    {
        tutorialParts[currentTutorial].completed = true;
        currentTutorial += 1;
        StartTutorialPart();
    }
}