using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject[] targets;
    public float speed = 2.0f;

    private float zOffset = 0.0f;

    private void Start()
    {
        // Calculate the Z offset based on the current camera angle (TODO testing purposes only!)
        zOffset = transform.position.y * Mathf.Tan(transform.rotation.x);

    }

    private void Update()
    {
        // This will keep the one player centered in the camera view
        Vector3 pos = transform.position;
        pos.z = Mathf.Lerp(transform.position.z, targets[0].transform.position.z - zOffset, speed * Time.deltaTime);
        pos.x = Mathf.Lerp(transform.position.x, targets[0].transform.position.x, speed * Time.deltaTime);

        transform.position = pos;
    }
}
