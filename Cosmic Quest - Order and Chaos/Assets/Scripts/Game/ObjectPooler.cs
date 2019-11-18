using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int pooledAmount;
    public GameObject pooledObject;
    public bool expandable;
    public List<GameObject> pooledObjects;
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
    
    private void Start()
    {
        foreach (ObjectPoolItem item in pooledItems)
        {
            for (int i = 0; i < item.pooledAmount; i++)
            {
                GameObject obj = Instantiate(item.pooledObject);
                obj.SetActive(false);
                item.pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        foreach (ObjectPoolItem item in pooledItems)
        {
            if (item.pooledObject == prefab)
            {
                foreach (var obj in item.pooledObjects)
                {
                    if (!obj.activeInHierarchy)
                    {
                        return obj;
                    }
                }
                
                // See if we can expand the pool if no objects available
                if (item.expandable)
                {
                    GameObject newObject = Instantiate(item.pooledObject);
                    newObject.SetActive(false);
                    item.pooledObjects.Add(newObject);
                    return newObject;
                }

                return null;
            }
        }

        return null;
    }
}
