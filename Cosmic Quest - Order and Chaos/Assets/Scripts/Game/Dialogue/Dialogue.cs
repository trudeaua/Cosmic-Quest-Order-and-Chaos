using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.Events;

=======
using UnityEngine.Events;

>>>>>>> 23058c66ae117a3642b7b3c0850286029b17584d
[System.Serializable]
public class Dialogue
{
    // Name of the character speaking
    public string name;

    // Dialogue to show
    [TextArea(3, 10)]
    public string[] sentences;

    public UnityEvent onComplete;
<<<<<<< HEAD
}
=======
}
>>>>>>> 23058c66ae117a3642b7b3c0850286029b17584d
