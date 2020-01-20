using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaosVoid : ISerializable
{
    // Reference to the scene for this chaos void
    public Scene ChaosVoidScene;
    
    // Whether this chaos void has been cleared
    public bool cleared { get; private set; }
    // Whether this chaos void has been started
    public bool started { get; private set; }
    // The final boss for this chaos void
    public GameObject boss { get; private set; }

    public void StartLevel()
    {
        // Load the scene
        // Initialize whatever...
        started = true;
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
