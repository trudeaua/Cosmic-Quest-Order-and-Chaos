using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleAnimate : MonoBehaviour
{
	public int size;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = Vector3.one * size + Vector3.one * Mathf.Sin(Time.time * 2)*size/2;
    }
}
