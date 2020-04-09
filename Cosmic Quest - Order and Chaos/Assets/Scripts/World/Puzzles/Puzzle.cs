using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
    public UnityEvent onReset;
    protected CharacterColour puzzleColour = CharacterColour.None;
    protected CharacterColour[] playerColours;

    public bool isComplete { get; private set; }

    protected virtual void Start()
    {
        playerColours = PlayerManager.Instance.CurrentPlayerColours;
    }

    /// <summary>
    /// Mark the puzzle as completed
    /// </summary>
    protected virtual void SetComplete()
    {
        isComplete = true;
        
        // Invoke any event functions
        onCompletion?.Invoke();
    }

    /// <summary>
    /// Set the colour of the puzzle
    /// </summary>
    /// <param name="colour">Colour that the puzzle's elements should be</param>
    public void SetPuzzleColour(CharacterColour colour)
    {
        puzzleColour = colour;
    }

    /// <summary>
    /// Reset the state of the puzzle
    /// </summary>
    public virtual void ResetPuzzle()
    {
        isComplete = false;
    }

    public string Serialize()
    {
        throw new System.NotImplementedException();
    }

    public void FromSerialized(string s)
    {
        throw new System.NotImplementedException();
    }
}
