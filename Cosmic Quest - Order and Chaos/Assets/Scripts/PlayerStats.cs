using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    public override void Die()
    {
        // Player death
        Debug.Log("Player died");
    }
}
