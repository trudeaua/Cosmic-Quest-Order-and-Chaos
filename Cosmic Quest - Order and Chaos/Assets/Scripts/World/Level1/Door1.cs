using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Door1 : Room
{
    private StringBuilder code;
    private StringBuilder input;

    // Start is called before the first frame update
    void Awake()
    {
        code = new StringBuilder("PGGP", 4);
        input = new StringBuilder("", 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (ArePlatformsActivated())
        {
            StartCoroutine(SetAnimTrigger());

            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

    // Returns whether all rocks have been positioned on their respective platforms
    public override bool ArePlatformsActivated ()
    {
        bool platformsActivated = true;

        if (m_Platforms == null || m_Platforms.Length == 0) return true;

        // Check if every platform in the room has a rock placed on it
        foreach (GameObject plat in m_Platforms)
        {
            if (!plat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlatformActivated"))
            {
                // If a platform hasn't been activated yet, return false
                platformsActivated = false;
            }
        }

        return platformsActivated;
    }
}
