using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    public int deadBoundary = 50;

    private Camera _camera;
    private float _playerHeight;
    private float _invTanOfView;
    private float _zOffset;
    private Vector3 _target;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (PlayerManager.Instance.Players.Count > 0)
            _playerHeight = PlayerManager.Instance.Players[0].GetComponent<CapsuleCollider>().height / 2f;
        else
            _playerHeight = 1f;

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

        // Set the initial camera target and move the camera to it
        _target = FindPlayersCenter();
        transform.position = new Vector3(_target.x, transform.position.y, _target.z - _zOffset);
    }

    private void FixedUpdate()
    {
        // Track the approximate center of the players
        _target = FindPlayersCenter();

        if (_target == Vector3.zero)
            return;

        Vector3 pos = transform.position;

        // TODO Smoothed motion of the camera causes slight stuttering in enemy moving
        pos.x = Mathf.Lerp(pos.x, _target.x, speed * Time.deltaTime);
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
        targetPos += new Vector3(0f, _playerHeight, 0f);
        Vector3 screenPos = _camera.WorldToScreenPoint(targetPos);
        Vector3 edgePos;

        // Handle clamping along the x-axis
        // TODO probably don't need to use ScreenToWorldPoint for x-axis?
        if (screenPos.x < deadBoundary)
        {
            // Left edge
            edgePos = _camera.ScreenToWorldPoint(new Vector3(deadBoundary, 0f, 0f));
            targetPos.x = edgePos.x;
        }
        else if (screenPos.x > Screen.width - deadBoundary)
        {
            // Right edge
            edgePos = _camera.ScreenToWorldPoint(new Vector3(Screen.width - deadBoundary, 0f, 0f));
            targetPos.x = edgePos.x;
        }

        // Handle clamping along the z-axis
        float scaledDeadBoundary = deadBoundary + (_playerHeight * _invTanOfView);
        if (screenPos.y < scaledDeadBoundary)
        {
            // Bottom edge
            edgePos = _camera.ScreenToWorldPoint(new Vector3(0f, scaledDeadBoundary, 0f));
            targetPos.z = edgePos.z + ((edgePos.y - targetPos.y) * _invTanOfView);
        }
        else if (screenPos.y > Screen.height - scaledDeadBoundary)
        {
            // Top edge
            edgePos = _camera.ScreenToWorldPoint(new Vector3(0f, Screen.height - scaledDeadBoundary, 0f));
            targetPos.z = edgePos.z + ((edgePos.y - targetPos.y) * _invTanOfView);
        }

        return targetPos - new Vector3(0f, _playerHeight, 0f); ;
    }
    /// <summary>
    /// Find the centre of all player positions
    /// </summary>
    /// <returns>The centre position of all active players</returns>
    private Vector3 FindPlayersCenter()
    {
        if (PlayerManager.Instance.Players.Count == 0)
        {
            // No players detected, so don't move camera target
            return Vector3.zero;
        }
        else if (PlayerManager.Instance.Players.Count == 1)
        {
            // Only one player, so simply follow them
            Vector3 pos = PlayerManager.Instance.Players[0].transform.position;
            return new Vector3(pos.x, 0, pos.z);
        }

        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        float zMin = float.MaxValue;
        float zMax = float.MinValue;

        foreach (GameObject player in PlayerManager.Instance.Players)
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

        Vector3 minPos = new Vector3(xMin, 0, zMin);
        Vector3 maxPos = new Vector3(xMax, 0, zMax);

        return (minPos + maxPos) * 0.5f;
    }
}
