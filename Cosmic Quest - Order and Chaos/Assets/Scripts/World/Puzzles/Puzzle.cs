using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
    public UnityEvent onReset;
    
    public bool IsComplete { get; private set; }

    protected void SetComplete()
    {
        IsComplete = true;
        
        // Invoke any event functions
        onCompletion?.Invoke();
    }

    protected void ResetPuzzle()
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
