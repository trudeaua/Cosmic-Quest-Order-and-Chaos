using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
	private bool active = false; // if the destination is filled.
	//this cannot be set by outside code. ONLY by proper item entering collider

	public string keyID; // MUST be the same as target object


	public bool getActive () 
	{
		return active;
	}

 //   void OnTriggerEnter(Collider other) //when something enters its collider
	//{
	//	if (!active && other.keyID == keyID ) // if it is not already full, and the keyIDs match
	//	{
	//		lockToPosition(other); //lock the other to its position
	//		active = true;//and set this destination to active
	//	}
	//}

	//private void lockToPosition(Collider other)
	//{
	//	//TODO, FUNCTIONALITY FOR THIS
	//	Debug.Log("Locked in " + other.name);
	//}

}
