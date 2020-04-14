using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("Should the music pause, and then resume when the dialogue completes?")]
    public bool pauseMusicOnPlay = false;
    public Dialogue dialogue;
    public void TriggerDialogue ()
    {
        // pauses music when dialogue is playing, resumes when dialogue completes
        if (pauseMusicOnPlay)
        {
            MusicManager.Instance.StopMusic();
            dialogue.onComplete.AddListener(MusicManager.Instance.PlayMusic);
        }
        // false to disable interactable dialogue
        DialogueManager.Instance.StartDialogue(dialogue, false); 
    }
}
