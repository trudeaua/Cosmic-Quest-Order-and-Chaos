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

    [HideInInspector]
    public GameObject[] players;

    private void Start()
    {
        // Make a list of all players in the game TODO temporary
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
