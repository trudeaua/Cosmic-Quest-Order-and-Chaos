using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPlatformManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var door = GameObject.Find("Door1");

        if (door != null)
        {
            Animator m_doorAnim;
            m_doorAnim.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Cannot find door1");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter (Collider other)
    {
        var rockColour = other.collider.tag;

        switch (rockColour)
        {
            case ("GreenRock"):
                Debug.Log("Green rock is on green platform");
                break;
            case ("PurpleRock"):
                Debug.Log("Purple rock is on purple platform");
                break;
        }
    }

}
