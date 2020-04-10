using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
	// the amount of damage to be done while contacting the hazard
	public int damageAmount;

    /// <summary>
    /// While colliding with the area, take damage over time
    /// </summary>
    protected virtual void OnTriggerStay(Collider other)
    {
    	if (other.transform.tag == "Player")
    	{  // if the player is 
    		other.GetComponent<PlayerStatsController>().TakeDamage(null,damageAmount,Time.deltaTime);
    	}
    }
}
