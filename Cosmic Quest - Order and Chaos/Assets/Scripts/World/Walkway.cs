using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkway : Interactable
{
    protected Collider m_Collider;  // Walkway collider - only let players of a matching colour pass
    
    void Awake()
    {
        m_Collider = transform.GetChild(0).gameObject.GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<EntityStatsController>().characterColour == this.colour)
        {
            // Disable collider for all players matching the colour
            m_Collider.enabled = false;
        }
        else
        {
            // Enable collider for players not matching colour
            m_Collider.enabled = true;
        }
    }
}
