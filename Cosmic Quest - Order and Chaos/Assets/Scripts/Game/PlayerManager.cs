using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    private static PlayerManager _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Only one player manager should be in the scene!");
    }
    #endregion

    public static readonly List<GameObject> Players = new List<GameObject>();

    public static void RegisterPlayer(GameObject player)
    {
        Players.Add(player);
    }

    public static void DeregisterPlayer(GameObject player)
    {
        Players.Remove(player);
    }
}
