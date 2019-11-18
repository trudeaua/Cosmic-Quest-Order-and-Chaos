using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Animator anim;

    public float TYPE_SPEED = 0.05f;
    private Queue<string> sentences;

    public static DialogueManager Instance = null;

    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(this);

        sentences = new Queue<string>();
        dialogueName.text = "";
        dialogueText.text = "";
    }

    public void StartDialogue(Dialogue dialogue, bool interactable)
    {
        anim.SetBool("IsShown", true);
        dialogueName.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(interactable);
    }

    public void DisplayNextSentence(bool interactable)
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // when user continues while sentence is animating
        StartCoroutine(TypeSentence(sentence));
        if (!interactable) StartCoroutine(AutoPlay(sentence));
    }

    IEnumerator AutoPlay(string sentence)
    {
        yield return new WaitForSeconds(sentence.Split(' ').Length / 2.5f);
        DisplayNextSentence(false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(TYPE_SPEED);
        }
    }

    void EndDialogue()
    {
        anim.SetBool("IsShown", false);
    }
}
