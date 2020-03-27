using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    // Name of the character speaking
    public string name;

    // Dialogue to show
    [TextArea(3, 10)]
    public string[] sentences;

    public UnityEvent onComplete;
}
