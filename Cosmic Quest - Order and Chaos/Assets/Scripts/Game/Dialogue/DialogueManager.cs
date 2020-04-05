﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Animator anim;
    public float TYPE_SPEED = 0.05f;
    public float AutoPlayTimeFactor = 2.5f;

    private Queue<string> sentences;
    private Dialogue CurrentDialogue;

    #region Singleton
    public static DialogueManager Instance = null;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(this);

        sentences = new Queue<string>();
        dialogueName.text = "";
        dialogueText.text = "";
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    /// <summary>
    /// Start showing dialogue
    /// </summary>
    /// <param name="dialogue">Dialogue object instance</param>
    /// <param name="interactable">Indicates whether the dialogue canvas is skippable or not</param>
    public void StartDialogue(Dialogue dialogue, bool interactable)
    {
        anim.SetBool("IsShown", true);
        dialogueName.text = dialogue.name;
        sentences.Clear();
        CurrentDialogue = dialogue;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(interactable);
    }

    /// <summary>
    /// Display the next sentence in the dialogue
    /// </summary>
    /// <param name="interactable">Indicates whether the dialogue canvas is skippable or not</param>
    public void DisplayNextSentence(bool interactable)
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            CurrentDialogue.onComplete.Invoke();
            return; 
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // when user continues while sentence is animating
        StartCoroutine(TypeSentence(sentence));
        if (!interactable) StartCoroutine(AutoPlay(sentence));
    }

    /// <summary>
    /// Auto play dialogue
    /// </summary>
    /// <param name="sentence">Sentence that will be shown next</param>
    /// <returns>An IEnumerator</returns>
    IEnumerator AutoPlay(string sentence)
    {
        yield return new WaitForSeconds(sentence.Split(' ').Length / AutoPlayTimeFactor);
        DisplayNextSentence(false);
    }

    /// <summary>
    /// Type the sentence on the dialogue canvas
    /// </summary>
    /// <param name="sentence">Sentence that will be shown next</param>
    /// <returns>An IEnumerator</returns>
    IEnumerator TypeSentence(string sentence)
    {
        // render the text behind the scenes, allows rich text effects to be applied nicely
        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;
        for (int i = 0; i < sentence.Length; i++)
        {
            int visibleCount = i % (sentence.Length + 1);
            dialogueText.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(TYPE_SPEED);
        }
    }
    /// <summary>
    /// Stop showing the dialogue canvas
    /// </summary>
    void EndDialogue()
    {
        anim.SetBool("IsShown", false);
    }
}
