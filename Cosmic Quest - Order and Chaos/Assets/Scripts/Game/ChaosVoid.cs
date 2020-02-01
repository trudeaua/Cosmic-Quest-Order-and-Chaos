using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChaosVoid : ScriptableObject, ISerializable
{
    // Reference to the scene for this chaos void
    public Object scene;
    // List of chaos voids required to be completed before starting this one
    public ChaosVoid[] prerequisites;
    
    // Whether this chaos void has been cleared
    public bool cleared { get; private set; }
    // Whether this chaos void has been started
    public bool started { get; private set; }
    // The final boss for this chaos void
    public GameObject boss { get; private set; }

    public void LoadLevel()
    {
        // Load the scene
        LevelManager.Instance.LoadYourAsyncScene(scene.name);
        // Initialize whatever...
        // Find all serializables in the level
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
