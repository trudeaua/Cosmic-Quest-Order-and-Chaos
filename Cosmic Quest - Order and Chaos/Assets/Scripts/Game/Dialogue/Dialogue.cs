using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // Name of the character speaking
    public string name;

    // Dialogue to show
    [TextArea(3, 10)]
    public string[] sentences;
}
