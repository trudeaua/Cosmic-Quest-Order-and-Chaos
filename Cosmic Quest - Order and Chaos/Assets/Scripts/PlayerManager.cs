using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
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
