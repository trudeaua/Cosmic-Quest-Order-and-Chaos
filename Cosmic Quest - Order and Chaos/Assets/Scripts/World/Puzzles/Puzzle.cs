using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
    public UnityEvent onReset;
    protected CharacterColour puzzleColour = CharacterColour.None;

    public bool isComplete { get; private set; }

    protected void SetComplete()
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

    public virtual void ResetPuzzle()
    {
        isComplete = false;
        onReset?.Invoke();
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
