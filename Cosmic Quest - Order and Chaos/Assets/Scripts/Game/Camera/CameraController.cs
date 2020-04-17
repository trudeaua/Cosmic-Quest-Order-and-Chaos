using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    [Range(0, 1)]
    public float deadBoundary = 0.05f;

    private Camera _camera;
    private float _playerHeight = 2.35f;
    private float _invTanOfView;
    private float _zOffset;
    private Vector3 _target;
    private bool xEnable = true;
    private bool yEnable = true;
    private Vector3 a;
    private Vector3 b;
    private Vector3 c;
    private Vector3 d;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        
        // Calculate the Z offset based on the current camera angle and height
        if (Mathf.Approximately(transform.rotation.eulerAngles.x, 90f))
        {
            // If looking straight down then there is no offset
            _zOffset = 0f;
            _invTanOfView = 1f;
        }
        else
        {
            // Cache the inverse tan of the camera angle for performance
            _invTanOfView = 1 / Mathf.Tan(transform.rotation.eulerAngles.x * Mathf.Deg2Rad);
            _zOffset = (transform.position.y - _playerHeight) * _invTanOfView;
        }
        
    }

    private void FixedUpdate()
    {
        a = _camera.ViewportToWorldPoint(new Vector3(deadBoundary, deadBoundary, _camera.transform.position.y));
        b = _camera.ViewportToWorldPoint(new Vector3(deadBoundary, 1 - deadBoundary, _camera.transform.position.y));
        c = _camera.ViewportToWorldPoint(new Vector3(1 - deadBoundary, deadBoundary, _camera.transform.position.y));
        d = _camera.ViewportToWorldPoint(new Vector3(1 - deadBoundary, 1 - deadBoundary, _camera.transform.position.y));
        // Track the approximate center of the players
        _target = FindPlayersCenter();

        if (_target == Vector3.zero)
            return;

        Vector3 pos = transform.position;
        if (xEnable)
            pos.x = Mathf.Lerp(pos.x, _target.x, speed * Time.deltaTime);
        if (yEnable)
            pos.z = Mathf.Lerp(pos.z, _target.z - _zOffset, speed * Time.deltaTime);

        transform.position = pos;
    }

    /// <summary>
    /// Clamps a given target position to a position within the screen boundaries.
    /// If the given position is not outside of the screen boundaries, then the same
    /// position is returned.
    /// </summary>
    /// <param name="targetPos">The position to clamp within the screen boundaries</param>
    /// <returns>The clamped position</returns>
    public Vector3 ClampToScreenEdge(Vector3 targetPos)
    {
        Vector3 pos = _camera.WorldToViewportPoint(targetPos);
        pos.x = Mathf.Clamp(pos.x, deadBoundary, 1 - deadBoundary);
        pos.y = Mathf.Clamp(pos.y, deadBoundary, 1 - deadBoundary);
        targetPos = _camera.ViewportToWorldPoint(pos);
        return targetPos;
    }
    
    /// <summary>
    /// Find the centre of all player positions
    /// </summary>
    /// <returns>The centre position of all active players</returns>
    private Vector3 FindPlayersCenter()
    {
        List<GameObject> players = PlayerManager.Instance.AlivePlayers;
        if (players.Count == 0)
        {
            // No players detected, so don't move camera target
            return Vector3.zero;
        }
        else if (players.Count == 1)
        {
            // Only one player, so simply follow them
            Vector3 pos = players[0].transform.position;
            return new Vector3(pos.x, 0, pos.z);
        }

        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;
        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                Vector3 playerPos = player.transform.position;
                xMin = Mathf.Min(xMin, playerPos.x);
                xMax = Mathf.Max(xMax, playerPos.x);
                zMin = Mathf.Min(zMin, playerPos.z);
                zMax = Mathf.Max(zMax, playerPos.z);
            }
        }
        // determine whether we should allow the camera to move along the x or y axis
        xEnable = !(xMin < a.x + 1  && xMax > c.x - 1);
        yEnable = !(zMin < a.z && zMax > b.z);
        Vector3 minPos = new Vector3(Mathf.Max(xMin, a.x), 0, Mathf.Max(zMin, a.z));
        Vector3 maxPos = new Vector3(Mathf.Min(xMax, c.x), 0, Mathf.Min(zMax, b.z));
        return (minPos + maxPos) * 0.5f;
    }
}
