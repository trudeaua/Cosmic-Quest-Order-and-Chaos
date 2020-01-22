using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaosVoid : ScriptableObject, ISerializable
{
    // Reference to the scene for this chaos void
    public Scene chaosVoidScene;
    
    // Whether this chaos void has been cleared
    public bool cleared { get; private set; }
    // Whether this chaos void has been started
    public bool started { get; private set; }
    // The final boss for this chaos void
    public GameObject boss { get; private set; }
    // The doors in this chaos void
    public GameObject[] doors { get; private set; }

    public void LoadLevel()
    {
        // Load the scene
        // Initialize whatever...
        // Find all doors in the scene
        // Find the final boss in the scene
        started = true;
    }

    public void LoadLevel(string saved)
    {
        
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
