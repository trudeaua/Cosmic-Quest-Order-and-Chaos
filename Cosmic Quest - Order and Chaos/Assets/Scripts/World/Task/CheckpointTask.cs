using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointTask : MonoBehaviour
{
    public UnityEvent onCheckpointReached;
    
    private List<GameObject> playersAtCheckpoint;

    private void Awake()
    {
        playersAtCheckpoint = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure we only register players once
        if (!other.CompareTag("Player") || playersAtCheckpoint.Contains(other.gameObject))
            return;
        
        playersAtCheckpoint.Add(other.gameObject);
        if (playersAtCheckpoint.Count == PlayerManager.Instance.NumPlayers)
        {
            onCheckpointReached.Invoke();
        }
    }
}
