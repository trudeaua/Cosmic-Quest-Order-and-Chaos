using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkway : MonoBehaviour
{
    public CharacterColour colour;
    
    private Collider _col;
    
    void Awake()
    {
        _col = transform.GetChild(0).gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EntityStatsController>().characterColour == colour)
        {
            // Disable collider for all players matching the colour
            _col.enabled = false;
        }
        else
        {
            // Enable collider for players not matching colour
            _col.enabled = true;
        }
    }
}
