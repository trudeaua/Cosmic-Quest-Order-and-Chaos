using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
    public UnityEvent onReset;
    protected CharacterColour[] puzzleColours;

    public bool isComplete { get; private set; }

    protected virtual void Start()
    {
        puzzleColours = PlayerManager.Instance.CurrentPlayerColours;
    }

    protected void SetComplete()
    {
        isComplete = true;
        
        // Invoke any event functions
        onCompletion?.Invoke();
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
