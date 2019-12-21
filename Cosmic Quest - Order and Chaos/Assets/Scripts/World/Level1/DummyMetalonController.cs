using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyMetalonController : MonoBehaviour
{
    public List<GameObject> dummyMetalons;

    void Awake()
    {
        dummyMetalons = new List<GameObject>();  

        Transform[] children = transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            GameObject obj = child.gameObject;

            if (obj.tag == "Enemy")
            {
                dummyMetalons.Add(obj);
            }
        }
        Debug.Log("metalons dummmies = " + dummyMetalons.Count);
    }

    void Update()
    {
        StartCoroutine(SetWalkPattern());
        enabled = false;
    }

    public virtual IEnumerator SetWalkPattern()
    {
        for (int i = 0; i < dummyMetalons.Count; i++)
        {
            if (i > 0)
            {
                Debug.Log("Entering forloop");
                yield return new WaitUntil(() => (dummyMetalons[i-1].transform.position.x >= -1.5f));
                dummyMetalons[i].GetComponent<DummyMetalon>().enabled = true;
            }
            else
            {
                dummyMetalons[i].GetComponent<DummyMetalon>().enabled = true;
            }
        }
        yield break;
    }
}
