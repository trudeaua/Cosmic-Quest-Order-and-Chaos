using System.Collections;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class ChaosVoid : ScriptableObject, ISerializable
{
    // Reference to the scene for this chaos void
    public SceneReference scene;

    // List of chaos voids required to be completed before starting this one
    public ChaosVoid[] prerequisites;

    // Returns whether all the prerequisite levels have been completed
    public bool isLocked
    {
        get
        {
            return prerequisites.Count(cv => cv.cleared) < prerequisites.Length;
        }
    }

    // Whether this chaos void has been cleared
    public bool cleared;

    // Whether this chaos void has been started
    public bool started;

    public void Initialize()
    {
        started = true;
    }

    public void Reset()
    {
        cleared = false;
        started = false;
    }

    public void Initialize(string saved)
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
