using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<GameObject> players;

    public void RegisterPlayer(GameObject player)
    {
        players.Add(player);
    }

    public void DeregisterPlayer(GameObject player)
    {
        players.Remove(player);
    }
}
