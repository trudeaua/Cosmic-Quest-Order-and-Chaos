using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public float maxStamina;
    public float currentStamina { get; private set; }

    public float maxMana;
    public float currentMana { get; private set; }
    
    protected override void Die()
    {
        Debug.Log("Player died");
        isDead = true;
        transform.gameObject.SetActive(false);
    }
}
