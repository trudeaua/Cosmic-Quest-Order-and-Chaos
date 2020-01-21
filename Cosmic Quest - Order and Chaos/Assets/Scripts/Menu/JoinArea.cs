using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoinArea : EventTrigger
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnMove(AxisEventData data)
    {
        if (Mathf.Abs(data.moveVector.x) > Mathf.Abs(data.moveVector.y))
        {
            Debug.Log("Switch Character");
        }
    }
}
