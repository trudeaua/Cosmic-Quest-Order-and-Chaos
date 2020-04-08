using UnityEngine;

public class PlayerUIControlSpawn : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.isTestInstance)
        {
            // If testing we spawn right away
            Spawn();
        }
        else
        {
            // Otherwise we're playing the build so we wait for scenes to load before spawning the UI controls
            LevelManager.Instance.loadingDoneEvent.AddListener(Spawn);
        }
    }

    /// <summary>
    /// Spawns a UI control for each player in the game
    /// </summary>
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