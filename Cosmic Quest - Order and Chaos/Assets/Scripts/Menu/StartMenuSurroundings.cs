using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSurroundings : MonoBehaviour
{
    public float rotationSpeed = 0.025f;
    
    private void Update()
    {
	    // Unity auto-loops the numbers, 179 goes to -179 degrees. If for some reason that doesn't happen,
	    // and the rotation value exceeds 360 there's a potential for it to just keep increasing and overflow.
	    // This is a guard against such overflow. It will simply stop spinning if that's the case.
    	if (transform.rotation.eulerAngles.y < 360)
    	{
	        transform.Rotate(0, rotationSpeed, 0, Space.Self);
    	}
    }
}
