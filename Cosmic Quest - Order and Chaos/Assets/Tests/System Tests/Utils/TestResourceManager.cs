using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestResourceManager : MonoBehaviour
{
    public List<TestResource> resources;
    
    #region Singleton

    public static TestResourceManager Instance;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public GameObject GetResource(string resourceName)
    {
        return (from resource in resources where resource.name.Equals(resourceName)
            select resource.gameObject).FirstOrDefault();
    }
}

[System.Serializable]
public struct TestResource
{
    public string name;
    public GameObject gameObject;
}
