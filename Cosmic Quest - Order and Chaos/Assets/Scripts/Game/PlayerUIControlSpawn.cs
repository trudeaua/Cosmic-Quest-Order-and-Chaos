using System.Collections;
using UnityEngine;

public class PlayerUIControlSpawn : MonoBehaviour
{
    [Tooltip("Seconds to wait for game state to not be loading")]
    public float timeout = 10;
    private void Start()
    {
        LevelManager.Instance.loadingDoneEvent.AddListener(Spawn);
    }
    private void Spawn()
    {
        int numPlayers = PlayerManager.Instance.NumPlayers;

        // Instantiate and place player characters evenly around spawn point
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject playerUIControl = PlayerManager.Instance.InstantiatePlayerUIControl(i);
        }
    }
}