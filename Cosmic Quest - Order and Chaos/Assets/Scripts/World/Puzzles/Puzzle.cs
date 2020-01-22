using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour, ISerializable
{
    public delegate void OnCompletion();
    public OnCompletion onCompletion;
    
    public bool isComplete { get; private set; }

    protected void SetComplete()
    {
        isComplete = true;
        
        // Invoke any delegate functions
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
