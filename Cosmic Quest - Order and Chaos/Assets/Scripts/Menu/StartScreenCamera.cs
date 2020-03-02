using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartScreenCamera : MonoBehaviour
{
    public float angle; //store angle of the camera
    void Update()
    {
    	if(transform.rotation.eulerAngles.y < 360) //unity auto-loops the numbers, 179 goes to -179 degrees. If for some reason that doesn't happen, and the rotation value exceeds 360 there's a potential for it to just keep increasing and overflow
    	{ //this is a guard against such overflow. It will simply stop spinning if that's the case. 
    		transform.Rotate(0,0.075f,0,Space.World); //will slowly do a full rotation through all y values.
    	}
        
    }
}
