using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 2.0f;

    private GameObject[] players;
    private float zOffset = 0.0f;
    private Vector3 target;

    private void Start()
    {
        // Grab player GameObjects from the player manager
        players = PlayerManager.instance.players;

        // Calculate the Z offset based on the current camera angle and height
        if (transform.rotation.eulerAngles.x == 90f)
            zOffset = 0f;
        else
            zOffset = transform.position.y * Mathf.Tan(transform.rotation.x);

        // Set the initial camera target and move the camera to it
        target = FindPlayersCenter();
        transform.position = new Vector3(target.x, transform.position.y, target.z - zOffset);
    }

    private void Update()
    {
        // Track the approximate center of the players
        target = FindPlayersCenter();

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(transform.position.x, target.x, speed * Time.deltaTime);
        pos.z = Mathf.Lerp(transform.position.z, target.z - zOffset, speed * Time.deltaTime);

        transform.position = pos;
    }

    private Vector3 FindPlayersCenter()
    {
        // No need to do these calculations if there's only one player
        if (players.Length == 1)
        {
            return new Vector3(players[0].transform.position.x, 0, players[0].transform.position.z);
        }

        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;

        foreach (GameObject player in players)
        {
            xMin = Mathf.Min(xMin, player.transform.position.x);
            xMax = Mathf.Max(xMax, player.transform.position.x);
            zMin = Mathf.Min(zMin, player.transform.position.z);
            zMax = Mathf.Max(zMax, player.transform.position.z);
        }

        Vector3 minPos = new Vector3(xMin, 0, zMin);
        Vector3 maxPos = new Vector3(xMax, 0, zMax);

        return (minPos + maxPos) * 0.5f;
    }
}
