using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom3 : Room
{
    // Start is called before the first frame update
    void Start()
    {
        m_Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (AreAllEnemiesKilled())
        {
            Debug.Log("All enemies killed - Open the door.");
            Anim.SetTrigger("OpenDoor");
            m_Collider.enabled = false;

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }
}
