using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 2.0f;

    private List<GameObject> _players;
    private float _zOffset;
    private Vector3 _target;

    private void Start()
    {
        // Grab player GameObjects from the player manager
        _players = PlayerManager.Instance.players;

        // Calculate the Z offset based on the current camera angle and height
        if (Mathf.Approximately(transform.rotation.eulerAngles.x, 90f))
            // If looking straight down then there is no offset
            _zOffset = 0f;
        else
            _zOffset = transform.position.y * Mathf.Tan(transform.rotation.x);

        // Set the initial camera target and move the camera to it
        _target = FindPlayersCenter();
        transform.position = new Vector3(_target.x, transform.position.y, _target.z - _zOffset);
    }

    private void FixedUpdate()
    {
        // Track the approximate center of the players
        _target = FindPlayersCenter();

        Vector3 pos = transform.position;

        // TODO Smoothed motion of the camera causes slight stuttering in enemy moving
        pos.x = Mathf.Lerp(transform.position.x, _target.x, speed * Time.deltaTime);
        pos.z = Mathf.Lerp(transform.position.z, _target.z - _zOffset, speed * Time.deltaTime);

        transform.position = pos;
    }

    private Vector3 FindPlayersCenter()
    {
        if (_players.Count == 1) // TODO temp
        {
            return new Vector3(_players[0].transform.position.x, 0, _players[0].transform.position.z);
        }

        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;

        foreach (GameObject player in _players)
        {
            if (player.activeSelf)
            {
                xMin = Mathf.Min(xMin, player.transform.position.x);
                xMax = Mathf.Max(xMax, player.transform.position.x);
                zMin = Mathf.Min(zMin, player.transform.position.z);
                zMax = Mathf.Max(zMax, player.transform.position.z);
            }
        }

        Vector3 minPos = new Vector3(xMin, 0, zMin);
        Vector3 maxPos = new Vector3(xMax, 0, zMax);

        return (minPos + maxPos) * 0.5f;
    }
}
