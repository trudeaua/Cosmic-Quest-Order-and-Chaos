using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
    public UnityEvent onReset;
    
    public bool IsComplete { get; private set; }

    protected CharacterColour[] playerColours;

    protected virtual void Start()
    {
        playerColours = PlayerManager.Instance.CurrentPlayerColours;
    }

    /// <summary>
    /// Mark the puzzle as completed
    /// </summary>
    protected virtual void SetComplete()
    {
        IsComplete = true;
        
        // Invoke any event functions
        onCompletion?.Invoke();
    }

    /// <summary>
    /// Reset the state of the puzzle
    /// </summary>
    public virtual void ResetPuzzle()
    {
        IsComplete = false;
    
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
