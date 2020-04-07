using System.Collections;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class EventHelper : ScriptableObject
{
    public void Victory()
    {
        GameManager.Instance.SetVictoryState();
    }
}
