using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int pooledAmount;
    public GameObject pooledObject;
    public bool expandable;
}

public class ObjectPooler : MonoBehaviour
{
    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<ObjectPoolItem> pooledItems;
    public List<GameObject> pooledObjects;
    
    private void Start()
    {
        pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItem item in pooledItems)
        {
            for (int i = 0; i < item.pooledAmount; i++)
            {
                GameObject obj = Instantiate(item.pooledObject);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy && obj == prefab)
            {
                return obj;
            }
        }

        foreach (ObjectPoolItem item in pooledItems)
        {
            if (item.pooledObject == prefab && item.expandable)
            {
                GameObject obj = Instantiate(item.pooledObject);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        
        return null;
    }
}
