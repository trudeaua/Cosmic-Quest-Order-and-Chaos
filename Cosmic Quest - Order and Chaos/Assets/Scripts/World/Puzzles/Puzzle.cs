using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
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
