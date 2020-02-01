using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour, ISerializable
{
    public UnityEvent onCompletion;
    
    public bool isComplete { get; private set; }

    protected void SetComplete()
    {
        isComplete = true;
        
        // Invoke any event functions
        onCompletion?.Invoke();
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
