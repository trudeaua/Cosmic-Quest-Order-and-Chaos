using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    public override void Die()
    {
        Debug.Log("Player died");
        isDead = true;
        transform.gameObject.SetActive(false);
    }
}
