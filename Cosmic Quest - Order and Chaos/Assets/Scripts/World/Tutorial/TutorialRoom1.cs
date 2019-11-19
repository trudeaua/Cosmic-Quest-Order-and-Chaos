using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom1 : Room
{
protected GameObject[] Platforms;

    // Start is called before the first frame update
    void Start()
    {
        Platforms = GameObject.FindGameObjectsWithTag("Platform");
        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AreRocksPositioned())
        {
            Debug.Log("All rocks activated - Open the door.");
            Anim.SetTrigger("OpenDoor");
            m_Collider.enabled = false;

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

    public override bool AreRocksPositioned ()
    {
        bool rocksPositioned = true;
        foreach (GameObject plat in Platforms)
        {
            if (!plat.GetComponent<Animator>().GetBool("RockPlaced"))
            {
                rocksPositioned = false;
            }
        }

        return rocksPositioned;
    }

}
