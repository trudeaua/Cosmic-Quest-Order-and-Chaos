using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    public float moveBoundary = 0.2f;
    public float deadBoundary = 0.05f;

    private Camera _camera;
    private List<GameObject> _players;
    private float _playerHeight;
    private float _invTanOfView;
    private float _zOffset;
    private Vector3 _target;

    private enum ScreenEdge
    {
        Top,
        Right,
        Bottom,
        Left
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        // Grab player GameObjects from the player manager
        _players = PlayerManager.players;

        if (_players.Count > 0)
            _playerHeight = _players[0].GetComponent<CapsuleCollider>().height / 2f;
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
            _zOffset = transform.position.y * _invTanOfView;
        }

        // Set the initial camera target and move the camera to it
        _target = FindPlayersCenter();
        transform.position = new Vector3(_target.x, transform.position.y, _target.z - _zOffset);
    }

    private void FixedUpdate()
    {
        // Track the approximate center of the players
        //_target = FindPlayersCenter();
        _target = CalculateEdgeOffset();

        if (_target == Vector3.zero)
            return;

        Vector3 pos = transform.position;

        // TODO Smoothed motion of the camera causes slight stuttering in enemy moving
        pos.x = Mathf.Lerp(pos.x, pos.x + _target.x, speed * Time.deltaTime);
        pos.z = Mathf.Lerp(pos.z, pos.z + _target.z, speed * Time.deltaTime);

        transform.position = pos;
    }

    private Vector3 CalculateEdgeOffset()
    {
        Vector3 positionOffset = Vector3.zero;
        Vector3 viewportOffset = Vector3.zero;

        byte deadZone = 0;

        foreach (GameObject player in _players)
        {
            Vector3 viewportPos = _camera.WorldToViewportPoint(player.transform.position + new Vector3(0f, _playerHeight, 0f));

            if (viewportPos.x < moveBoundary)
            {
                // Player is on the left of the screen
                viewportOffset.x -= moveBoundary - viewportPos.x;

                if (viewportPos.x < deadBoundary)
                    deadZone |= 1 << (int)ScreenEdge.Left;
            }
            else if (viewportPos.x > 1f - moveBoundary)
            {
                // Player is on the right of the screen
                viewportOffset.x += viewportPos.x - (1f - moveBoundary);

                if (viewportPos.x > 1f - deadBoundary)
                    deadZone |= 1 << (int)ScreenEdge.Right;
            }

            if (viewportPos.y < moveBoundary)
            {
                // Player is on the bottom of the screen
                viewportOffset.y -= moveBoundary - viewportPos.y;

                if (viewportPos.x < deadBoundary)
                    deadZone |= 1 << (int)ScreenEdge.Bottom;
            }
            else if (viewportPos.y > 1f - moveBoundary)
            {
                // Player is on the top of the screen
                viewportOffset.y += viewportPos.y - (1f - moveBoundary);

                if (viewportPos.y > 1f - deadBoundary)
                    deadZone |= 1 << (int)ScreenEdge.Top;
            }
        }

        // Calculate world space movement required
        if (viewportOffset != Vector3.zero)
        {
            // Handle any movements against dead zones
            if (((deadZone & 1 << (int)ScreenEdge.Left) > 0 && viewportOffset.x > 0f) ||
                ((deadZone & 1 << (int)ScreenEdge.Right) > 0 && viewportOffset.x < 0f))
            {
                viewportOffset.x = 0f;
            }
            else if (((deadZone & 1 << (int)ScreenEdge.Top) > 0 && viewportOffset.y > 0f) ||
                     ((deadZone & 1 << (int)ScreenEdge.Bottom) > 0 && viewportOffset.y < 0f))
            {
                viewportOffset.y = 0f;
            }

            Vector3 start = _camera.ViewportToWorldPoint(Vector3.zero);
            Vector3 end = _camera.ViewportToWorldPoint(viewportOffset);
            positionOffset = end - start;
            positionOffset.y = 0f;
        }

        return positionOffset;
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
        Vector3 viewportPos = _camera.WorldToViewportPoint(targetPos);
        Vector3 edgePos;

        // Handle clamping along the x-axis
        if (viewportPos.x < deadBoundary)
        {
            // Left edge
            edgePos = _camera.ViewportToWorldPoint(new Vector3(deadBoundary, 0f, 0f));
            targetPos.x = edgePos.x;
        }
        else if (viewportPos.x > 1f - deadBoundary)
        {
            // Right edge
            edgePos = _camera.ViewportToWorldPoint(new Vector3(1f - deadBoundary, 0f, 0f));
            targetPos.x = edgePos.x;
        }

        // Handle clamping along the z-axis
        if (viewportPos.y < deadBoundary)
        {
            // Bottom edge
            edgePos = _camera.ViewportToWorldPoint(new Vector3(0f, deadBoundary, 0f));
            targetPos.z = edgePos.z + ((edgePos.y - targetPos.y) * _invTanOfView);
        }
        else if (viewportPos.y > 1f - deadBoundary)
        {
            // Top edge
            edgePos = _camera.ViewportToWorldPoint(new Vector3(0f, 1f - deadBoundary, 0f));
            targetPos.z = edgePos.z + ((edgePos.y - targetPos.y) * _invTanOfView);
        }

        return targetPos - new Vector3(0f, _playerHeight, 0f); ;
    }

    private Vector3 FindPlayersCenter()
    {
        if (_players.Count == 1)
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
