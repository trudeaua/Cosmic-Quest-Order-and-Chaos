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
    private int numPlayers;
    private TutorialPart[] tutorialParts;
    public DialogueTrigger mageDialogue;
    public DialogueTrigger meleeDialogue;
    public DialogueTrigger healerDialogue;
    public DialogueTrigger rangedDialogue;
    // Start is called before the first frame update
    void Start()
    {
        // Wait for players to be spawned
        PlayerSpawn.onPlayersSpawned.AddListener(SetupTutorialParts);
    }

    private void SetupTutorialParts()
    {
        numPlayers = PlayerManager.Instance.NumPlayers;
        tutorialParts = new TutorialPart[numPlayers];
        Debug.Log(numPlayers);
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTutorialPart(int playerNumber)
    {
        tutorialParts[playerNumber].dialogueTrigger.TriggerDialogue();
    }
}
