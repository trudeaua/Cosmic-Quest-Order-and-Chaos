using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Vector3 pos;

    void Start()
    {
        pos = new Vector3(-2.39f, -10.3f, -51.18f);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Teleport metalon");
        other.transform.position = other.GetComponent<DummyMetalon>()._position;
    }

}
